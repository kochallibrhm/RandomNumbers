# Random Numbers Game

## Summary
The Random Numbers Game is a web application that allows users to generate random numbers between 0 and 100. The user with the highest number wins the match.

## Functional Requirements
The application works as follows:
1. **Current Match**: The next expiring match is defined as the current match.
2. **Anonymous Users**: If a user is not logged in, they can see all matches played and their winners.
3. **Playing the Game**: A logged-in user can play by clicking on a “Play Now” button which generates a random number and associates it with the user for the current match.
4. **Match Storage**: The system stores a list of pre-defined matches, each with an expiry timestamp up to when submissions are accepted. The match winner is announced once the timestamp expires.
5. **Single Submission**: Once a user submits a number, they cannot play again until the match expires.
6. **Logged-in Users**:
   - Can see all matches played and their winners, similar to anonymous users.
   - Can refresh the results by clicking on a “Refresh Results” button.
   - Can play the current match. The screen shows the expiry timestamp of the current match and a “Play Now” button. Once the button is clicked, the generated number is displayed and the button is hidden.
7. **Authentication**: Only logged-in users can play the game.

## Technical Requirements
- The screen does not auto-refresh to display results. Once a user clicks “Play Now”, they have to wait until the match expires and then click “Refresh Results” to see if they won.
- Match results are stored in MS SQL, and all database communication is handled through Entity Framework Core.
- The client-side technology used is Angular.
- Server-side logic is written in ASP.NET Core Web API.
- The code is structured as if it were built for production.

## Setup Instructions

### Backend Setup
1. **Connection String**: Update the connection string in `appsettings.json` according to your database configuration.
2. **Database**: Ensure there is no existing database with the name `CleverbitCodingTask`. If it exists, delete or rename it.
3. **CORS Configuration**: To avoid CORS policy restrictions, pass the address of your Angular application as a parameter to the `WithOrigins` method within the `AddCors` block in the `Startup.cs` file.
4. **Game Duration**: To change the game duration, update the `GameTimeMinutes` parameter within the `MatchBackgroundService`.
5. **Swagger UI**: Use Swagger UI to test and review the API methods. It appears when you launch the backend project.

### Frontend Setup
1. **API URL**: Edit the `apiUrl` in the `environment.ts` file according to your API address.
2. **Dependencies**: No extra npm packages are required.

## Development Notes
- The focus during development was to deliver a functional application that meets the requirements within the given timeframe. Some parts could be written more cleanly and in accordance with best practices.
- Thank you for your review and feedback. Feel free to reach out to me via email if you have any questions: kochallibrhm@gmail.com

Best regards,
[Halil İbrahim]
