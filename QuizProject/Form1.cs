using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace QuizProject
{
    public class Question
    {
        public string Text { get; }
        public string[] Answers { get; }
        public int[] CorrectIndexes { get; }
        public bool IsMultipleChoice { get; }

        public Question(string text, string[] answers, int[] correctIndexes, bool isMultipleChoice)
        {
            Text = text;
            Answers = answers;
            CorrectIndexes = correctIndexes;
            IsMultipleChoice = isMultipleChoice;
        }
    }

    public partial class Form1 : Form
    {
        private int currentQuestionIndex = 0;
        private List<Question> questions;
        private List<Control> answerControls = new List<Control>();
        private int correctAnswersCount = 0;

        public Form1()
        {
            InitializeComponent();
            LoadQuestions();
            LoadQuestion();
        }

        private void LoadQuestions()
        {
            questions = new List<Question>
            {
                new Question("What is 5 + 7?", new string[] { "10", "11", "12" }, new int[] { 2 }, false),
                new Question("Select even numbers:", new string[] { "2", "3", "5", "6" }, new int[] { 0, 3 }, true),
                new Question("Capital of Italy?", new string[] { "Rome", "Berlin", "Madrid" }, new int[] { 0 }, false),
                new Question("What is 4 x 4?", new string[] { "12", "14", "16" }, new int[] { 2 }, false),
                new Question("Which are web browsers?", new string[] { "Chrome", "Java", "Firefox", "C#" }, new int[] { 0, 2 }, true),
                new Question("What is 20 / 4?", new string[] { "4", "5", "6" }, new int[] { 1 }, false),
                new Question("Which colors are in the CMYK model?", new string[] { "Cyan", "Magenta", "Yellow", "Red" }, new int[] { 0, 1, 2 }, true),
                new Question("Smallest planet?", new string[] { "Mercury", "Venus", "Earth" }, new int[] { 0 }, false),
                new Question("Who developed Java?", new string[] { "Oracle", "Sun Microsystems", "IBM" }, new int[] { 1 }, false),
                new Question("Which are vegetables?", new string[] { "Carrot", "Tomato", "Cucumber", "Apple" }, new int[] { 0, 2 }, true)

            };
        }

        private void LoadQuestion()
        {
            if (currentQuestionIndex >= questions.Count)
            {
                DisplaySummary();
                return;
            }

            var question = questions[currentQuestionIndex];
            QuestionLabel.Text = question.Text;

            foreach (var control in answerControls)
            {
                Controls.Remove(control);
                control.Dispose();
            }
            answerControls.Clear();

            int yPosition = QuestionLabel.Bottom + 10;
            for (int i = 0; i < question.Answers.Length; i++)
            {
                Control answerControl = question.IsMultipleChoice ? (Control)new CheckBox() : new RadioButton();
                answerControl.Text = question.Answers[i];
                answerControl.Tag = question.CorrectIndexes.Contains(i) ? "1" : "0";
                answerControl.Left = QuestionLabel.Left;
                answerControl.Top = yPosition;
                answerControl.AutoSize = true;
                answerControl.ForeColor = System.Drawing.Color.AliceBlue;
                Controls.Add(answerControl);
                answerControls.Add(answerControl);
                yPosition += 30;
            }
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            bool isCorrect = true;
            foreach (var control in answerControls)
            {
                bool isChecked = (control is RadioButton rb && rb.Checked) || (control is CheckBox cb && cb.Checked);
                bool shouldBeChecked = control.Tag.ToString() == "1";

                if (isChecked != shouldBeChecked)
                {
                    isCorrect = false;
                    break;
                }
            }
            if (isCorrect)
                correctAnswersCount++;

            if (currentQuestionIndex < questions.Count - 1)
            {
                currentQuestionIndex++;
                LoadQuestion();
            }
            else
            {
                DisplaySummary();
            }
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (currentQuestionIndex > 0)
            {
                currentQuestionIndex--;
                LoadQuestion();
            }
        }

        private void DisplaySummary()
        {
            QuestionLabel.Text = "Quiz Completed!";
            foreach (var control in answerControls)
            {
                Controls.Remove(control);
                control.Dispose();
            }
            answerControls.Clear();

            Label summaryLabel = new Label();
            summaryLabel.Text = $"You answered {correctAnswersCount} out of {questions.Count} questions correctly.";
            summaryLabel.Left = QuestionLabel.Left;
            summaryLabel.Top = QuestionLabel.Bottom + 20;
            summaryLabel.AutoSize = true;
            summaryLabel.BackColor = System.Drawing.Color.AliceBlue;
            Controls.Add(summaryLabel);

            NextBtn.Text = "Close";
            NextBtn.Click += (s, e) => this.Close();
            PrevBtn.Visible = false;
        }
    }


}


