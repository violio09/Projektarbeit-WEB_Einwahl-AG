-- Ändere deine Wahl an einer Kurswahl --

UPDATE schuhler
SET s_erstWahl = <Kurswahl>
WHERE s_ID = <schülerID>; 

UPDATE schuhler
SET s_zweitWahl = <Kurswahl>
WHERE s_ID = <schülerID>;

UPDATE schuhler
SET s_drittWahl = <Kurswahl>
WHERE s_ID = <schülerID>;

-- -------------------------------------------------------- --

-- Ändere den Namen des Jeweiligen Schühlers --

UPDATE schuhler
SET s_Name = '<NeuerName>', s_Vorname = '<NeuerVorname>'
WHERE s_ID = <schülerID>;
-- -------------------------------------------------------- --
