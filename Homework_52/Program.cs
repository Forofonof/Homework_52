using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        PatientFactory patientFactory = new PatientFactory();
        Database database = new Database(patientFactory);
        Hospital hospital = new Hospital(database);
        Menu menu = new Menu(hospital);

        database.AddPatientsInList();
        menu.Work();
    }
}

class Menu
{
    private readonly Hospital _hospital;

    public Menu(Hospital hospital)
    {
        _hospital = hospital;
    }

    public void Work()
    {
        const string SortFullName = "1";
        const string SortAge = "2";
        const string SearchDisease = "3";

        bool isWork = true;

        Console.WriteLine("\nМеню больницы к вашим услугам!\n");

        while (isWork)
        {
            Console.WriteLine($"{SortFullName} - Отсортировать всех больных в алфавитном порядке");
            Console.WriteLine($"{SortAge} - Отсортировать всех больных по возрасту.");
            Console.WriteLine($"{SearchDisease} - Вывести больных с определенным заболеванием.");

            string userInput = Console.ReadLine();

            switch (userInput)
            {
                case SortFullName:
                    _hospital.ShowPatientsByFullName();
                    break;

                case SortAge:
                    _hospital.ShowPatientsByAge();
                    break;

                case SearchDisease:
                    _hospital.ShowPatientsByDisease();
                    break;

                default:
                    Console.WriteLine("Ошибка! Нет такой команды!");
                    break;
            }
        }
    }
}

class Hospital
{
    private readonly Database _database;

    public Hospital(Database database)
    {
        _database = database;
    }

    public void ShowPatientsByFullName()
    {
        var filteredPatients = _database.GetPatients().OrderBy(patient => patient.FullName).ToList();

        ShowPatients(filteredPatients);
    }

    public void ShowPatientsByAge()
    {
        var filteredPatients = _database.GetPatients().OrderBy(patient => patient.Age).ToList();

        ShowPatients(filteredPatients);
    }

    public void ShowPatientsByDisease()
    {
        Console.WriteLine("Введите болезнь:");
        string userInput = Console.ReadLine();

        var filtredPatients = _database.GetPatients().Where(patient => patient.Disease.Equals(userInput)).ToList();

        ShowPatients(filtredPatients);
    }

    private void ShowPatients(List<Patient> patients)
    {
        if (patients.Count > 0)
        {
            foreach (Patient patient in patients)
            {
                patient.ShowInfo();
            }
        }
        else
        {
            Console.WriteLine("Совпадений не найдено.");
        }
    }
}

class Database
{
    private readonly List<Patient> _patients = new List<Patient>();
    private readonly PatientFactory _patientFactory;

    public Database(PatientFactory patientFactory)
    {
        _patientFactory = patientFactory;
    }

    public List<Patient> AddPatientsInList()
    {
        _patients.AddRange(_patientFactory.CreateMultiplePatients());

        return _patients;
    }

    public IReadOnlyList<Patient> GetPatients()
    {
        return _patients.AsReadOnly();
    }
}

class PatientFactory
{
    private readonly string[] _diseases = { "Сахарный диабет", "Простуда", "Корона", "Мигрень", "Грипп", "Гастрит" };

    private readonly string[] _fullNames = 
    {
        "Артышкина Екатерина", "Соколов Артём", "Гусева Амина", "Соколова Ева", "Кузнецов Кирилл", "Иванова Марина",
        "Смирнов Иван", "Форофонов Вячеслав", "Ларионова Елизавета", "Родин Марк", "Балашова Ксения", "Фролова Елизавета"
    };

    private readonly Random _random = new Random();

    public List<Patient> CreateMultiplePatients()
    {
        int numberOfPatients = 15;

        List<Patient> patients = new List<Patient>();

        for (int i = 0; i < numberOfPatients; i++)
        {
            Patient patient = CreatePatient();
            patients.Add(patient);
        }

        return patients;
    }

    private Patient CreatePatient()
    {
        int ageMinimum = 18;
        int ageMaximum = 70;

        int age = _random.Next(ageMinimum, ageMaximum);
        string name = _fullNames[_random.Next(_fullNames.Length)];
        string disease = _diseases[_random.Next(_diseases.Length)];

        return new Patient(name, age, disease);
    }
}

class Patient
{
    public Patient(string name, int age, string disease)
    {
        FullName = name;
        Age = age;
        Disease = disease;
    }

    public int Age { get; private set; }

    public string FullName { get; private set; }

    public string Disease { get; private set; }

    public void ShowInfo()
    {
        Console.WriteLine($"Имя|Фамилия - {FullName}. Возраст - {Age}. Заболевание - {Disease}.\n");
    }
}