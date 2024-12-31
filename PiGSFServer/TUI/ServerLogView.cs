﻿using PiGSF.Server;
using Terminal.Gui;
using Attribute = Terminal.Gui.Attribute;

public class ServerLogView : View
{
    private readonly TextView _textView;

    public ServerLogView(Server server)
    {
        // Window
        ServerLogger.logWindow = this;
        // ColorScheme = new ColorScheme(new Attribute(Color.White, Color.DarkGray));
        // X = 6+numLogs;
        // Y = numLogs;
        // Width = Dim.Fill() - 6;
        // Height = Dim.Fill() - 6;
        // BorderStyle = LineStyle.Double;
        // ShadowStyle = ShadowStyle.Transparent;

        // Textview
        _textView = new TextView
        {
            X = 0,
            Y = 0,
            Width = Dim.Fill(),
            Height = Dim.Fill(),
            ReadOnly = true,
            WordWrap = true,
            ColorScheme = new ColorScheme(new Attribute(Color.White, 0)),
        };
        _textView.VerticalScrollBar.Visible = true;

        Add(_textView);

        postInit();
    }

    async void postInit()
    {
        await Task.Yield();
        RefreshLogs();
    }

    public void RefreshLogs()
    {
        var logs = ServerLogger.messages;
        _textView.Text = string.Join("\n", logs);
        _textView.MoveEnd(); // Scroll to the end
    }
}