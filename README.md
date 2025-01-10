# SportLive

SportLive is a desktop application built using **WPF** and **C#**. It provides live updates on ongoing matches, allows users to add their favorite teams, and integrates with **Kafka** for real-time match updates.

## Features

1. **Add Teams**
   - Users can add their favorite teams to track their matches.

2. **Live Match Updates**
   - Displays a list of ongoing matches with live updates.
   - Updates are received in real-time using **Kafka**.

3. **User-Friendly Interface**
   - Built using WPF for a modern and interactive UI.

## Architecture

### Components

- **WPF UI**
  - Provides a clean and intuitive interface for users to interact with the application.

- **C# Backend**
  - Handles business logic, data processing, and Kafka integration.

- **Kafka Integration**
  - Uses Kafka consumers to fetch real-time match updates.

### Data Flow

1. **Adding Teams**
   - User inputs team details via the UI.
   - Details are stored in a local or remote database.

2. **Fetching Live Updates**
   - Kafka consumers listen to match topics.
   - Updates are processed and displayed in the UI.

3. **Displaying Matches**
   - Matches are displayed in a list with live scores and status.

## Development

### Tech Stack

- **Frontend**: WPF
- **Backend**: C#
- **Real-Time Updates**: Kafka
- **Database**: SQLite or MSSQL (optional)

### Kafka Integration

- Ensure Kafka is running and topics are configured.
- Use a consumer to listen for updates from the `live-matches` topic.

## License

This project is licensed under the [MIT License](LICENSE).

---

For questions or suggestions, feel free to contact us at support@sportlive.com.
