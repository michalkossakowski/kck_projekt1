truncate table  [kck_projektDb].[dbo].[Categories]

INSERT INTO [kck_projektDb].[dbo].[Categories] ([Name])
VALUES 
('Praca'),
('Osobiste'),
('Zakupy'),
('Podróże'),
('Zdrowie'),
('Sport'),
('Hobby'),
('Finanse'),
('Technologia'),
('Edukacja');

truncate table [kck_projektDb].[dbo].[Notes]

INSERT INTO [kck_projektDb].[dbo].[Notes] ([AuthorId], [Title], [Content], [ModifiedDate], [CategoryId])
VALUES 
(1, 'Plan spotkań', 'Przygotować prezentację na zebranie', '2025-02-02', 1),
(1, 'Zakupy lista', 'Mleko, chleb, kawa, warzywa', '2025-02-05', 3),
(1, 'Weekend trip', 'Zarezerwować hotel w górach', '2025-02-10', 4),
(1, 'Lekarz wizyta', 'Umówić się na kontrolę', '2025-02-15', 5),
(1, 'Miesięczny rap.', 'Przygotować podsumowanie wyników', '2025-02-20', 1),
(1, 'Mama urodziny', 'Kupić prezent i kwiaty', '2025-02-25', 2),
(1, 'Nowa książka', 'Sprawdzić nowości w księgarni', '2025-03-01', 2),
(1, 'Plan urlopu', 'Wybrać termin i miejsce wakacji', '2025-03-06', 4),
(1, 'Bieganie plan', 'Rozpisać plan treningowy', '2025-02-03', 6),
(1, 'Nowy budżet', 'Przeanalizować wydatki na luty', '2025-02-06', 8),
(1, 'Serwis auta', 'Zaplanować wymianę oleju', '2025-02-09', 2),
(1, 'Lista filmów', 'Spisać filmy do obejrzenia', '2025-02-12', 7),
(1, 'Przegląd tech', 'Sprawdzić nowinki technologiczne', '2025-02-18', 9),
(1, 'Kurs online', 'Zapisać się na nowy kurs', '2025-02-22', 10),
(1, 'Praca raport', 'Przygotować raport kwartalny', '2025-02-27', 1),
(1, 'Ćwiczenia jogi', 'Rozpocząć poranną jogę', '2025-02-28', 6),
(1, 'Książki must', 'Lista książek do przeczytania', '2025-03-02', 7),
(1, 'Nowy telefon', 'Porównać modele przed zakupem', '2025-03-05', 9);
