# Masjid Description Fix

## Issue

The "About This Masjid" section in the masjid details page was showing the address instead of a proper description.

## Root Cause

In the `MasjidRepository.AddAsync()` method, when creating new masjids, the description was being set to the address instead of a meaningful description.

## Solution Implemented

### 1. Backend Fix

- Updated the `AddAsync` method in `MasjidRepository.cs` to set a proper default description instead of the address
- New masjids will now have a meaningful description like: "A beautiful masjid located in [address]. This sacred place of worship serves the local Muslim community and visitors from around the world."

### 2. Frontend Fix

- Added a `getDisplayDescription()` method in `MasjidDetailComponent` that provides a fallback description when:
  - The description is empty
  - The description is just the address
- Updated the HTML template to use this method instead of directly displaying `localizedDescription`

### 3. Database Update (Optional)

For existing masjids that still have the address as their description, you can run the following SQL script:

```sql
-- Update existing masjid descriptions that are just the address
UPDATE mc
SET mc.Description = CONCAT('A beautiful masjid located in ', m.Address, '. This sacred place of worship serves the local Muslim community and visitors from around the world.')
FROM MasjidContents mc
INNER JOIN Masjids m ON mc.MasjidId = m.Id
WHERE mc.Description = m.Address
   OR mc.Description IS NULL
   OR mc.Description = '';

-- For masjids that don't have any content records, create them
INSERT INTO MasjidContents (MasjidId, LanguageId, Name, Description)
SELECT
    m.Id,
    l.Id,
    m.ShortName,
    CONCAT('A beautiful masjid located in ', m.Address, '. This sacred place of worship serves the local Muslim community and visitors from around the world.')
FROM Masjids m
CROSS JOIN Languages l
WHERE l.Code = 'en'
  AND NOT EXISTS (
    SELECT 1 FROM MasjidContents mc
    WHERE mc.MasjidId = m.Id AND mc.LanguageId = l.Id
  );
```

## Files Modified

- `Backend/Repositories/Implementations/MasjidRepository.cs` - Fixed default description
- `src/app/Features/Masjid-Details/Masjid-Details.component.ts` - Added fallback method
- `src/app/Features/Masjid-Details/Masjid-Details.component.html` - Updated template

## Result

- New masjids will have proper descriptions
- Existing masjids with address-only descriptions will show a meaningful fallback description
- The "About This Masjid" section will now display useful information instead of just the address
