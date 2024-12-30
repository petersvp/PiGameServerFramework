﻿using System.Linq;
using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

public class LogWindow : Window
{
    private readonly TextView _textView;
    private readonly RoomLogger _logger;

    static int numLogs = 0;
    public LogWindow(string title, PiGSF.Server.Room room)
    {
        ++numLogs;
        // Window
        Title = title;
        _logger = room.Log;
        ColorScheme = new ColorScheme(new Attribute(Color.White, Color.DarkGray));
        X = 6+numLogs;
        Y = numLogs;
        Width = Dim.Fill() - 6;
        Height = Dim.Fill() - 6;
        BorderStyle = LineStyle.Double;
        ShadowStyle = ShadowStyle.Transparent;

        // Textview
        _textView = new TextView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ReadOnly = true,
            WordWrap = false,
            ColorScheme = new ColorScheme(new Attribute(Color.White, 0x444444))
        };


        Add(_textView);

        room.Log.logWindow = this;

        KeyDown += (s, e) =>
        {
            if (e.KeyCode == KeyCode.Esc)
            {
                --numLogs;
                room.Log.logWindow = null;
                Application.Top.Remove(this);
                Dispose();
            };
        };

        postInit();
    }

    async void postInit()
    {
        await Task.Yield();
        RefreshLogs();
    }

    public void RefreshLogs()
    {
        var logs = _logger.messages;
        _textView.Text = string.Join("\n", logs);
        _textView.MoveEnd(); // Scroll to the end
    }
}