# Deep Fake Game

A Unity-based game project featuring user account creation, high score tracking, and SQLite-based persistence.

---

## Requirements

- **Unity Editor**: Version 2022.3.49f1 recommended
- **.NET Framework**: Unity-managed (no separate install required)
- **IDE**: (Optional) Visual Studio or JetBrains Rider for code editing and debugging
- **SQLite Support**: Included in the project via plugins or DLLs (see `Assets/Plugins/`)

---

## How to Run the Game

Unzip and open the Deep Fake Game application under Releases on Github. 

**To open the game in Unity:**

1. **Clone the repository**

   ```bash
   git clone https://github.com/yourusername/deep-fake-game.git
   cd deep-fake-game

2. **Open the project in Unity**
* Launch Unity Hub
* Click Add project
* Select the root folder of this repository

3. Play the game
*  Open the Login Scene in Assets/Scenes/Login.unity

Running Unit Tests
This project uses the Unity Test Framework with NUnit for automated testing.

## Running tests
1. Open the project in Unity

2. Go to Window → General → Test Runner

3. Choose Play Mode

4. Click Run All

Test files are located in:
Assets/Scripts/Tests/
