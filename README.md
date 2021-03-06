# [htunes](https://github.com/ShepherdJerred/htunes)
A Windows application for playing and organizing music

![Screenshot of hTunes](https://i.imgur.com/nj1K3r5.png)

### Known bugs
* None

### Extra credit
* Rename/delete playlists
* ControlTemplate

### Contributions
Levi Mason (50%)
* Toolbar at top
    * Add song
        * Open dialog box
            * .mp3, .m4a, .wma, .wav
        * Load metadata from music file
        * Select song in DataGrid after adding
    * Add playlist
        * Add song to end of playlist by dragging from DataGrid
        * Started coding Drag/Drop
    * About dialog
        * Dialog box, not message box
* LastFM API
    * Load all song data on program startup
    * Load song data as needed when adding songs
* ControlTemplate for look/behavior of playback buttons (Extra credit) and most buttions
* Playlist context menu (Extra credit)
    * Rename
        * Open dialog box, enter new playlist name
        * Validate name (No blank or identical names)
    * Delete

Jerred Shepherd (50%)
* DataGrid
    * Show DetailsPane when song is selected
    * View songs in playlist "in order of position"
    * Context menu
        * Remove song from all music
            * Confirm
        * Remove song from playlist
            * Perserve song order when removing
    * Modify song information in DataGrid when viewing all music
        * Not editable when viewing playlist
* Song playback
    * Play next song in list (Optional)
* Playback buttons
    * Play
    * Stop
    * Pause (Optional)
    * Change volume (Optional)
    * Previous/Next (Optional)
* Store songs in music.xml
    * Save music.xml when changes are made
* Resizable main window
    * Stretch/shrink contents
* Playlist ListBox
    * List all music
    * List playlists
    * Need some way to communicate selected item between ListBox and DataGrid
    * Drag and Drop
