namespace KYPCOBA9l
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            MessageBox.Show("Добро пожаловать!\nЗагрузка...\n\n\n(Для продолжения нажмите \"OK\")", "Наша маленькая пиццерия...");
            MessageBox.Show("Сейчас вы увидите окно заполнения тестовыми данными!\n\n\n(Для продолжения нажмите \"OK\")", "Наша маленькая пиццерия...");
            ApplicationConfiguration.Initialize();
            Application.Run(new Form0());
        }
    }
}