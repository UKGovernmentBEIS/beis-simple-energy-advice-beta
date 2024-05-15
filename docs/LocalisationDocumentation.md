# Localisation Documentation

## Pull Request Checklist
Please ensure the following points have been followed when making changes. These will help to maintain the integrity and tidiness of the resource files and the translation:

- [ ] If I have <b>added</b> a new string to the code, I have checked it is surrounded by an appropriate localiser
- [ ] If I have <b>added</b> a new string to the code, I have added a new key/value set to the corresponding resource file
- [ ] If I have <b>added</b> a new string to the code, and/or a new key to resource file, it is <b>not</b> the direct English text, and follows the format of "\[GeneralPurposeOfText\]String" e.g. "WelcomePageTitleString"
- [ ] If I have <b>added</b> a new key to a resource file, I have checked it is not a duplicate (case insensitively)
- [ ] If I have <b>edited</b> a resource file key, I have checked that all references have also been updated
- [ ] If I have <b>edited</b> a resource file value, I have checked that all references to its key are still correct with the updated value
- [ ] If I have <b>edited</b> a string in code, I have checked that it still corresponds to a resource value key, and have created an appropriate new key and value set if not
- [ ] If I have <b>deleted</b> a string from the code, I have checked in the corresponding resource file that it is still being referenced elsewhere, and deleted it if it is not
- [ ] If I have interacted with strings or resource files during this ticket, I have visually confirmed that the strings/resources involved translate as expected
