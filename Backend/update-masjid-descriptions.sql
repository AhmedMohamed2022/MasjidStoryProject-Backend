-- Update existing masjid descriptions that are just the address
-- This script will update MasjidContent records where the description equals the address

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