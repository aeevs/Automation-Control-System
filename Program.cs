namespace KYPCOBA9l
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            MessageBox.Show("����� ����������!\n��������...\n\n\n(��� ����������� ������� \"OK\")", "���� ��������� ��������...");
            MessageBox.Show("������ �� ������� ���� ���������� ��������� �������!\n\n\n(��� ����������� ������� \"OK\")", "���� ��������� ��������...");
            ApplicationConfiguration.Initialize();
            Application.Run(new Form0());
        }
    }
}