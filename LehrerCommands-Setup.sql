-- Vorarbeit fuer die Abfrageaufgaben -- 

CREATE TABLE `schuhler` (
  `s_ID` int(11) NOT NULL,
  `s_Name` varchar(50) NOT NULL,
  `s_Vorname` varchar(50) NOT NULL,
  `s_Klasse` int(11) NOT NULL,
  `s_erstWahl` int(11) DEFAULT NULL,
  `s_zweitWahl` int(11) DEFAULT NULL,
  `s_drittWahl` int(11) DEFAULT NULL,
  PRIMARY KEY (`s_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

INSERT INTO `schuhler` (`s_ID`, `s_Name`, `s_Vorname`,`s_Klasse`, `s_erstWahl`, `s_zweitWahl`, `s_drittWahl`) VALUES
(1, 'Fiur', 'Jan', 10, 3, 5, 2),
(2, 'Carstens', 'Max', 9, 5, 1, 4),
(3, 'Huß', 'Lena', 11, 3, 1, 2),
(4, 'Bolleininger', 'Larissa', 13, 1, 2, 3),
(5, 'Stahl', 'Elias', 9, 4, 2, 1),
(6, 'Kisla', 'Sahin', 10, 5, 1, 3);

-- Erstellung der Kurse --

CREATE TABLE `kurse` (
  `k_ID` int(11) NOT NULL,
  `k_Name` varchar(50) NOT NULL,
  `k_MaxTeilnehmer` int(11) DEFAULT '10',
  PRIMARY KEY (`k_ID`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;

INSERT INTO `kurse` (`k_ID`, `k_Name`, `k_MaxTeilnehmer`) VALUES
(1, 'Java', 4),
(2, 'NoSQL', 2),
(3, 'Rust Programmierung', 7),
(4, 'Web3 and Blockchain', 8),
(5, 'Linux Development', 6);

-- -------------------------------------------------------- --

-- Abfragen ( Lehrer ) --

-- Wer hat welchen Kurs als Erstwahl angegeben? --

SELECT s.s_Name as Schühlername, k.k_Name as Kursname
FROM schuhler as s, kurse as k
WHERE s.s_erstwahl LIKE k.k_ID AND k.k_ID = <Kurswahl>;

-- Welche Kurse wurden wie oft als Erstwahl angegeben? --

SELECT k.k_Name as Kursname, COUNT(s.s_ID) as Anzahl
FROM schuhler as s, kurse as k
WHERE s.s_erstwahl LIKE k.k_ID
GROUP BY k.k_Name;

-- Bekomme den max anzahl der möglichen Teilnehmer eines Kurses --

SELECT k.k_Name as Kursname, k.k_MaxTeilnehmer as MaxTeilnehmer
FROM kurse as k
WHERE k.k_ID = <Kurswahl>;

-- Bekomme die Anzahl Erstwahlen, Zweitwahlen und Drittwahlen aller Kurses --

SELECT k.k_Name as Kursname, 
       SUM(CASE WHEN s.s_erstwahl = k.k_ID THEN 1 ELSE 0 END) as Erstwahlen,
       SUM(CASE WHEN s.s_zweitwahl = k.k_ID THEN 1 ELSE 0 END) as Zweitwahlen,
       SUM(CASE WHEN s.s_drittwahl = k.k_ID THEN 1 ELSE 0 END) as Drittwahlen
FROM kurse as k
LEFT JOIN schuhler as s ON s.s_erstwahl = k.k_ID OR s.s_zweitwahl = k.k_ID OR s.s_drittwahl = k.k_ID
GROUP BY k.k_Name;  

-- Bekomme die Anzahl der Schüler pro Klasse --

SELECT s.s_Klasse as Klasse, COUNT(s.s_ID) as AnzahlSchüler
FROM schuhler as s
GROUP BY s.s_Klasse;    

-- Erstelle einen neuen Kurs --

INSERT INTO `kurse` (`k_ID`, `k_Name`, `k_MaxTeilnehmer`) VALUES
(7, 'Python Programmierung', 10);

-- Lösche einen Kurs -- 

DELETE FROM `kurse`
WHERE k_ID = <Kurswahl>;

-- Finde heraus welcher Schühler keine Wahl angegeben hat --  

SELECT s.s_ID as SchühlerID, s.s_Name as Schühlername, s.s_Vorname as SchühlerVorname
FROM schuhler as s
WHERE s.s_erstwahl IS NULL OR s.s_zweitwahl IS NULL OR s.s_drittwahl IS NULL;

-- Ändere alle Kurse eines Schühlers auf NULL --

UPDATE schuhler
SET s_erstWahl = NULL, s_zweitWahl = 5, s_drittWahl = 2
WHERE s_ID = <schülerID>; 

-- Erstelle eine Spalte für die Usernamen und Passworter --

ALTER TABLE `schuhler`
ADD COLUMN `s_Username` varchar(50) DEFAULT NULL,
ADD COLUMN `s_Password` varchar(50) DEFAULT NULL; 

-- Füge Usernamen und Passwort für einen Schühler hinzu --

UPDATE schuhler
SET s_Username = 'fiurj', s_Password = 'amongus123'
WHERE s_ID = 1;

-- Entferne den Usernamen und Passwort eines Schühlers --

UPDATE schuhler
SET s_Username = NULL, s_Password = NULL
WHERE s_ID = 1;
