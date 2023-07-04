# VisualizadorLaberintos

## Introduction

Desktop application **made entirely in Unity*** with the purpose of being a final degree project. Focused on the **Algorithms for Videogames** subject, taught in the Videogame Design and Development Degree at the Rey Juan Carlos University, this application works as an **interactive tool for future students**.

The entire application is summarized in several interfaces where any student will be able to carry out a study of **the most used search algorithms**. Focusing from video games, it has been decided to choose a fairly common environment: **a maze**. In this way, viewing the execution of each one of the chosen search algorithms, every **user will be able to see how the exit from the maze is found** with the calculations within the code and in an image approach.

This type of tool have been a common application throughout the university degree, serving as **a great help with other types of environments or programming problems** . For this reason, we have sought to choose a tool that can be useful for subsequent generations of students, just as others have served for our promotion. With this, it is possible to unite interesting concepts such as **video games and programming**, encouraging students to continue investigating algorithms.

## Instructions

Being an application focused on **interaction with the user and interfaces**, it will be important to know the flow through them. For this reason it will be convenient to address each of the screens separately and in an orderly manner.

1- The **first interface** that can be seen when start the application is the logo, with the name of the application, and a button to go to the next interface.

<p align="center"> <img width="809" height="423" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG1.png"></p>

2- This interface contains **all the elements to be able to customize** the maze problem that you want to try to solve. There are two dropdown menus: one with the dimensions of the maze and another with the algorithm you want to use to solve it. The application has been designed so that **the upper left corner** is always the beginning and **the lower right corner** is always the end. This decision may be changed in future updates.

The maze will be created randomly in the following interface with the dimensions that are chosen, either 3x3, 4x4 or 6x6. The three search algorithms chosen will be **two focused on graphs** (width and depth) and **one heuristic** (A star). When the parameters that the user wants have been chosen, the continue button is pressed.

<p align="center"> <img width="809" height="423" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG2.png"></p>
<p align="center"> <img width="809" height="423" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG3.png"></p>

3- When the user enters this interface from the selection menu, the first thing he will see is **a button to create the maze**. Pressing it will create, internally, the structure that stores the labyrinth. The maze will be visible **on the right side** and a button to solve it.

Pressing the solve button will cause the maze to be solved internally, in **order to store in a data structure** the order of the maze run according to the chosen algorithm. After this, the button to view the solution becomes visible.


The visualization button will **allow the navigation menu to appear** with the buttons in charge of moving forward or backward in the resolution of the maze, keeping in mind the origin and the goal.

<p align="center"> <img width="809" height="423" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG4.png"></p>

4- From this moment on, the user will be able to **progress in solving the maze**, analyzing each case through a succession of states:

  - The program checks if **it is in the goal**
  - If it finds the goal, the algorithm will stop, but if it doesn't, **it will look at the next cell**, depending on the chosen algorithm.
  - The current cell is marked as visited and **the next option is marked to repeat** the entire process.

The user **can advance or go back in the search**, being able to wait longer where he has doubts. Each of these states will be accompanied by a pseudo code of what is being done at each moment. All these code extracts have a much more literary explanation.

<p align="center"> <img width="680" height="280" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG5.png"></p>
<p align="center"> <img width="680" height="280" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG6.png"></p>

5-When the goal is found, **everything ends and a window appears indicating the number of steps** you have taken towards the goal with that method. The user can choose to repeat that maze with another method. In the maze it will see **the number of decisions** that have led to the right path and those that have not, through **a color code**.

<p align="center"> <img width="729" height="382" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG7.png"></p>
<p align="center"> <img width="250" height="250" src="https://raw.githubusercontent.com/SergYeah55/VisualizadorLaberintos/master/IMAGES/IMG8.png"></p>

All this application **has been created for educational purposes**, so the options for future updates are quite wide.

## Credits

  - *Concept Design*: <a href="https://github.com/SergYeah55">SergYeah55</a>.
  - *Programmers*: <a href="https://github.com/SergYeah55">SergYeah55</a>.
  - *Visual Design*: <a href="https://github.com/SergYeah55">SergYeah55</a>.
