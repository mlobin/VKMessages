# VKMessages
Parser and analyser for VK messages. Uses C# variant of VK API

## How it works
Works by parsing ~200 messages, analysing them (smiles, pictures and general messages count), and saving to JSON.

## Issues
Skips unaccessible messages - this is intended behaviour since the addition of message deletion.

## TBD
Add authorisation and custom filters for messages.
