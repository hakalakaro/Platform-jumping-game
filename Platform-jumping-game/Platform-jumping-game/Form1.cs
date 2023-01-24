using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Platform_jumping_game
{
    public partial class Form1 : Form
    {
        bool goLeft, goRight, jumping, isGameOver;

        // Player
        int jumpSpeed;
        int force;
        int score = 0;
        int playerSpeed = 7;

        // Moving platforms
        int horizontalSpeed = 5;
        int verticalSpeed = 3;

        // Enemies
        int enemyOneSpeed = 5;
        int enemyTwoSpeed = 3;

        public Form1()
        {
            InitializeComponent();
        }

        // main
        private void MainGameTimer(object sender, EventArgs e)
        {
            // display score
            txtScore.Text = "Score: " + score;

            player.Top += jumpSpeed;

            if (goLeft == true)
            {
                player.Left -= playerSpeed;
            }
            if (goRight == true)
            {
                player.Left += playerSpeed;
            }
            if (jumping == true && force < 0)
            {
                jumping = false;
            }
            if (jumping == true)
            {
                jumpSpeed = -8;
                force -= 1;
            }
            else
            {
                jumpSpeed = 10;
            }

            // Goes through every control in the form application with different parameters
            foreach (Control x in this.Controls)
            {   // Here we use pictureboxes
                if (x is PictureBox)
                {   // Here we use pictureboxes with a tag of platform
                    if ((string)x.Tag == "platform")
                    {   // If player model "hits" platforms
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {   // Set force and set player model on top of a platform it touched
                            force = 8;
                            player.Top = x.Top - player.Height;
                            // Move player along the moving horizontal platform
                            if ((string)x.Name == "horizontalPlatform" && goLeft == false || (string)x.Name == "horizontalPlatform" && goRight == false)
                            {
                                player.Left -= horizontalSpeed;
                            }
                        }
                        x.BringToFront();
                    }
                    // Here we choose pictureboxes with a tag of coin
                    if ((string)x.Tag == "coin")
                    {   // If player model "hits" a coin
                        if (player.Bounds.IntersectsWith(x.Bounds) && x.Visible == true)
                        {   // Remove the coin and add +1 to score
                            x.Visible = false;
                            score++;
                        }
                    }
                    // Here we choose pictureboxes with a tag of enemy
                    if ((string)x.Tag == "enemy")
                    {   // If player model "hits" an enemy
                        if (player.Bounds.IntersectsWith(x.Bounds))
                        {   // stop game, display a message on death
                            gameTimer.Stop();
                            isGameOver = true;
                            txtScore.Text = "Score: " + score + Environment.NewLine + "You died, try again!";
                        }
                    }
                }
            }
            // Describes the movement of horizontal platform
            horizontalPlatform.Left -= horizontalSpeed;

            if (horizontalPlatform.Left < 0 || horizontalPlatform.Left + horizontalPlatform.Width > this.ClientSize.Width)
            {
                horizontalSpeed = -horizontalSpeed;
            }
            // Describes the movement of vertical platform
            verticalPlatform.Top += verticalSpeed;

            if (verticalPlatform.Top < 195 || verticalPlatform.Top > 586)
            {
                verticalSpeed = -verticalSpeed;
            }
            // Describes the movement of enemy number one
            enemyOne.Left -= enemyOneSpeed;

            if (enemyOne.Left < pictureBox5.Left || enemyOne.Left + enemyOne.Width > pictureBox5.Left + pictureBox5.Width)
            {
                enemyOneSpeed = -enemyOneSpeed;
            }
            // Describes the movement of enemy number two
            enemyTwo.Left += enemyTwoSpeed;

            if (enemyTwo.Left < pictureBox2.Left || enemyTwo.Left + enemyTwo.Width > pictureBox2.Left + pictureBox2.Width)
            {
                enemyTwoSpeed = -enemyTwoSpeed;
            }
            // If the player goes under a certain height (falling death)
            if (player.Top + player.Height > this.ClientSize.Height + 50)
            {   // End game, display death message
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "You died, try again!";
            }
            // If player reaches the green tube and has collected all the coins
            if (player.Bounds.IntersectsWith(door.Bounds) && score == 25)
            {   // End game, display success message
                gameTimer.Stop();
                isGameOver = true;
                txtScore.Text = "Score: " + score + Environment.NewLine + "Level complete!";
            }
            // Display message of the task needed to complete before going through the tube
            else
            {
                txtScore.Text = "Score: " + score + Environment.NewLine + "Collect all 25 coins";
            }
        }

        // On key press functions
        private void KeyIsDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = true;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = true;
            }
            if (e.KeyCode == Keys.Space && jumping == false)
            {
                jumping = true;
            }
        }

        private void KeyIsUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                goLeft = false;
            }
            if (e.KeyCode == Keys.Right)
            {
                goRight = false;
            }
            if (jumping == true)
            {
                jumping = false;
            }
            // On enter restarts the game
            if (e.KeyCode == Keys.Enter && isGameOver == true)
            {
                RestartGame();
            }
        }

        // Function to restart the game
        private void RestartGame()
        {   // Reset movement and score
            jumping = false;
            goLeft = false;
            goRight = false;
            score = 0;

            txtScore.Text = "Score: " + score;
            
            // Reset collected coins back to start settings
            foreach (Control x in this.Controls)
            {
                if (x is PictureBox && x.Visible == false)
                {
                    x.Visible = true;
                }
            }

            // Reset the position of player, platform and enemies back to start settings
            player.Left = 12;
            player.Top = 709;
            
            enemyOne.Left = 415;
            enemyOne.Top = 431;

            enemyTwo.Left = 338;
            enemyTwo.Top = 621;

            horizontalPlatform.Left = 219;
            verticalPlatform.Top = 586;

            // Starts the game again
            gameTimer.Start();
        }
    }
}
