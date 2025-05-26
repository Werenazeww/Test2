using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;

namespace TestApp
{
    public partial class MainWindow : Window
    {
        private List<Question> _questions = new List<Question>();
        private int _currentQuestionIndex = 0;
        private int _score = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void AddQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(QuestionTextBox.Text))
            {
                MessageBox.Show("Введите вопрос!");
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
                },
                CorrectAnswerIndex = int.Parse(CorrectAnswerTextBox.Text) - 1
            };

            _questions.Add(question);
            StatusText.Text = $"Добавлено вопросов: {_questions.Count}";
            QuestionTextBox.Clear();
        }

        private void SubmitAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (_questions.Count == 0)
            {
                MessageBox.Show("Сначала добавьте вопросы!");
                return;
            }
        
            int selectedIndex = GetSelectedAnswerIndex();
            if (selectedIndex == _questions[_currentQuestionIndex].CorrectAnswerIndex)
                _score++;

       
            _currentQuestionIndex++;
            if (_currentQuestionIndex < _questions.Count)
                ShowQuestion(_currentQuestionIndex);
            else
                ShowResult();
        }

        private int GetSelectedAnswerIndex()
        {
            if (Answer1Radio.IsChecked == true) return 0;
            if (Answer2Radio.IsChecked == true) return 1;
            if (Answer3Radio.IsChecked == true) return 2;
            return 3;
        }

        private void ShowQuestion(int index)
        {
            var question = _questions[index];
            CurrentQuestionText.Text = question.Text;
            Answer1Radio.Content = question.Options[0];
            Answer2Radio.Content = question.Options[1];
            Answer3Radio.Content = question.Options[2];
            Answer4Radio.Content = question.Options[3];
        }

        private void ShowResult()
        {
            ResultText.Text = $"Тест завершён! Правильных ответов: {_score} из {_questions.Count}";
            _currentQuestionIndex = 0;
            _score = 0;
        }
    }
}