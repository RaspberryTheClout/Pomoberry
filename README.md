# Pomoberry

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

A Pomodoro timer application for Windows, built with C# and .NET 8.

This project is built with a decoupled architecture, separating the core timer logic from the user interface. The primary goal is to create a simple, reliable tool for time management using the Pomodoro Technique.

## Features

The `Pomoberry.Core` library provides the main functionality:
* Configure durations for work and break sessions.
* Set the total number of sessions to complete.
* Standard timer controls: Start, Pause, and Reset.
* Events for UI integration: `TimeChanged`, `SessionStarted`, and `AllSessionsCompleted`.