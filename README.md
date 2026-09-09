# Sudoku Solver

**This project is no longer under active development.**

## About

This is an old project originally written in 2023 being uploaded to GitHub now. It's a C# console application that solves Sudoku puzzles by reading the board directly off the screen and typing the solution back in. It was built as a personal project to experiment with basic computer vision (via pixel color matching) and a classic backtracking solver.

## Goal

The goal was to build a bot that could fully solve a Sudoku puzzle without any input other than pointing it at an open game window found [here](https://sudoku.com/), using a mix of:

- **Screen reading** — capturing a region of the screen and inspecting individual pixels to figure out which digit (if any) is in each of the 81 tiles.
- **Solving logic** — a recursive backtracking algorithm that fills in the empty tiles while respecting standard Sudoku rules (no repeats in a row, column, or 3x3 box).
- **Simulated input** — sending keystrokes to type the solved digits back into the page.

## Implementation

Most of the core goal was completed:

- The bot locates the game board by scanning for a known line color near the cursor.
- Each of the 81 tiles is read by sampling a handful of pixel offsets that are only ever a background color when a specific digit *isn't* present, letting it tell all nine digits apart without doing any actual image recognition.
- The board is solved with a straightforward recursive backtracking search.
- The solution is entered by clicking the first tile, then using arrow keys and number keys to snake through the grid typing in every digit.

Warnings:

- The tile/line colors and pixel offsets are hardcoded to one specific Sudoku skin, so it only works with the [website](https://sudoku.com/) color scheme.
- There's no validation that the puzzle was read correctly — if a tile is misread, the solver will either fail to find a solution or enter an incorrect one.
- It assumes a 9x9 classic Sudoku with no variant rules.

## Usage

This project targets Windows only, since it relies on `System.Windows.Forms` and `System.Drawing` for screen capture and simulated keyboard/mouse input.

1. Navigate to [https://sudoku.com/](https://sudoku.com/) and open a puzzle.
2. Build and run the project.
3. When prompted in the console, hover your cursor over the board and press Enter, then press Enter again to let it search for and lock onto the board's grid lines.
4. Press any key to start. The bot will read the board, solve it, and type the solution in automatically.

[Simple Showcase Video](https://www.youtube.com/watch?v=XGkZtz7pf7A)
