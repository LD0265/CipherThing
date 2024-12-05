# CipherThing
Been playin around with substitution ciphers recently


Run the executable in the command line and use '-h' or '-help' for usage

`Example:  .\CipherThing.exe -d -i .\test\output.txt -o test -sk`

`This will read output.txt in the test folder and overwrite output.txt in the same folder as output, it will also show the key at the end `

The encoder uses a key provided or randomly generated to encode text

The decoder uses frequency analysis to crack encoded text ( if any letter is missing from the input, the output will be mostly unreadable !!! )
