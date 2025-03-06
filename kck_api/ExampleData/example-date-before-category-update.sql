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

INSERT INTO [kck_projektDb].[dbo].[Notes] ([AuthorId], [Title], [Content], [ModifiedDate], [CategoryId])
VALUES 
-- 2020
(1, 'Nowy Rok 2020', 'Cele na nowy rok', '2020-01-01', 2),
(1, 'Budżet luty', 'Analiza wydatków', '2020-02-10', 8),
(1, 'Ćwiczenia plan', 'Ustalić grafik ćwiczeń', '2020-03-15', 6),
(1, 'Zakupy lato', 'Letnie ubrania i akcesoria', '2020-06-20', 3),
(1, 'Wakacje plan', 'Zaplanować wyjazd wakacyjny', '2020-07-05', 4),
(1, 'Powrót do szk.', 'Zakup książek i przyborów', '2020-08-25', 10),
(1, 'Jesienne menu', 'Nowe przepisy na jesień', '2020-10-10', 2),
(1, 'Święta 2020', 'Lista prezentów świątecznych', '2020-12-05', 2),

-- 2021
(1, 'Cele 2021', 'Plany na nowy rok', '2021-01-02', 2),
(1, 'Auto serwis', 'Zaplanować przegląd auta', '2021-04-18', 2),
(1, 'Sportowy cel', 'Biegać 3x w tygodniu', '2021-05-07', 6),
(1, 'Kurs online', 'Znaleźć kurs programowania', '2021-09-14', 10),
(1, 'Kino jesień', 'Sprawdzić premiery filmowe', '2021-11-22', 7),

-- 2022
(1, 'Nowy Rok 2022', 'Ustalić postanowienia', '2022-01-03', 2),
(1, 'Finanse luty', 'Analiza oszczędności', '2022-02-28', 8),
(1, 'Bieg na 5km', 'Przygotować się do zawodów', '2022-06-10', 6),
(1, 'Podróż góry', 'Sprawdzić szlaki w Tatrach', '2022-08-05', 4),
(1, 'Jesień plany', 'Zrobić listę celów na Q4', '2022-09-20', 1),
(1, 'Prezenty 2022', 'Kupić świąteczne upominki', '2022-12-10', 2),

-- 2023
(1, 'Ćwiczenia luty', 'Rozpisać nowy plan treningowy', '2023-02-08', 6),
(1, 'Podróż maj', 'Plan na długi weekend', '2023-05-15', 4),
(1, 'Budżet lipiec', 'Przeanalizować wydatki wakacyjne', '2023-07-25', 8),
(1, 'Książki lato', 'Lista do przeczytania', '2023-08-11', 7),
(1, 'Kino zimowe', 'Sprawdzić grudniowe premiery', '2023-12-01', 7),

-- 2024
(1, 'Nowy Rok 2024', 'Cele na rok', '2024-01-05', 2),
(1, 'Dieta plan', 'Przygotować zdrowe menu', '2024-03-22', 5),
(1, 'Nowy telefon', 'Znaleźć najlepszy model', '2024-06-14', 9),
(1, 'Sport jesień', 'Zapisać się na siłownię', '2024-09-09', 6),
(1, 'Zakupy Black', 'Lista na Black Friday', '2024-11-27', 3),