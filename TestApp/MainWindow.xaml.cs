using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace TestApp
{
    public partial class MainWindow : Window
    {
        private List<Question> questions = new List<Question>();
        private int currentQuestionIndex = 0;
        private int correctAnswers = 0;
        private bool testInProgress = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                StatusText.Text = "Введите текст вопроса!";
                return;
            }

            var question = new Question
            {
                Text = QuestionTextBox.Text,
                Options = new List<string>
                {
                    Option1TextBox.Text,
                    Option2TextBox.Text,
                    Option3TextBox.Text,
                    Option4TextBox.Text
                }
            };

            if (!int.TryParse(CorrectAnswerTextBox.Text, out int correctIndex) ||
                correctIndex < 1 || correctIndex > 4)
            {
                StatusText.Text = "Номер правильного ответа должен быть от 1 до 4!";
                return;
            }

            question.CorrectAnswerIndex = correctIndex - 1; 
            questions.Add(question);


            QuestionTextBox.Clear();
            Option1TextBox.Text = "Вариант 1";
            Option2TextBox.Text = "Вариант 2";
            Option3TextBox.Text = "Вариант 3";
            Option4TextBox.Text = "Вариант 4";
            CorrectAnswerTextBox.Text = "1";

            StatusText.Text = $"Добавлено вопросов: {questions.Count}";
        }

        private void StartTest_Click(object sender, RoutedEventArgs e)
        {
            if (questions.Count == 0)
            {
                ResultText.Text = "Нет вопросов для тестирования!";
                return;
            }

            testInProgress = true;
            currentQuestionIndex = 0;
            correctAnswers = 0;

            QuestionGroupBox.Visibility = Visibility.Visible;
            SubmitButton.Visibility = Visibility.Visible;
            ResultText.Text = "";

            ShowQuestion(questions[0]);
        }

        private void ShowQuestion(Question question)
        {
            CurrentQuestionText.Text = $"{currentQuestionIndex + 1}. {question.Text}";
            Answer1Radio.Content = question.Options[0];
            Answer2Radio.Content = question.Options[1];
            Answer3Radio.Content = question.Options[2];
            Answer4Radio.Content = question.Options[3];


            Answer1Radio.IsChecked = Answer2Radio.IsChecked =
                Answer3Radio.IsChecked = Answer4Radio.IsChecked = false;
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (!testInProgress) return;


            if (!(Answer1Radio.IsChecked ?? false) && !(Answer2Radio.IsChecked ?? false) &&
                !(Answer3Radio.IsChecked ?? false) && !(Answer4Radio.IsChecked ?? false))
            {
                MessageBox.Show("Выберите ответ!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            int selectedIndex = Answer1Radio.IsChecked == true ? 0 :
                               Answer2Radio.IsChecked == true ? 1 :
                               Answer3Radio.IsChecked == true ? 2 : 3;

            if (selectedIndex == questions[currentQuestionIndex].CorrectAnswerIndex)
            {
                correctAnswers++;
            }

            currentQuestionIndex++;
            if (currentQuestionIndex < questions.Count)
            {
                ShowQuestion(questions[currentQuestionIndex]);
            }
            else
            {
                FinishTest();
            }
        }

        private void FinishTest()
        {
            testInProgress = false;
            QuestionGroupBox.Visibility = Visibility.Collapsed;
            SubmitButton.Visibility = Visibility.Collapsed;

            ResultText.Text = $"Тест завершен!\n" +
                              $"Правильных ответов: {correctAnswers} из {questions.Count}\n" +
                              $"Результат: {(correctAnswers * 100 / questions.Count)}%";
        }
    }
}