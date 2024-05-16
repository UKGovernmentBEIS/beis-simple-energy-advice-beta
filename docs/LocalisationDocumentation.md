# Localisation Documentation
## Recommended Tool

When interacting with resource files, it is recommended that you use [Localisation Manager](https://www.jetbrains.com/help/rider/Resources__LocalizationManager.html) in Rider, as opposed to making manual changes to any resource files. It is also recommended that you ensure you only have <b> one </b> file selected in the file tree on the left hand side of the Localisation manager at once (unless you are searching) to avoid making unintended changes to other files. 

It also more intuitively displays the Key/English Value/Welsh Value triad abstraction used elsewhere in the documentation, as opposed to the reality of there being two (`.resx` and `.resx.cy`) files having two separate Key/Value pairs.

Note: One limitation to the Localisation Manager found is lack of clarity around duplicated keys. These are not clearly visible, and when trying to find and fix them, it can be easier to use Ctrl + F to search the files and make edits manually.

## When making changes to Find Ways To Save:
Make sure you use the following guidance when making changes to the Find Ways To Save project.
This guidance is designed to help you to maintain the integrity and tidiness of the resource files and the translation.


If in the process of following one piece of guidance below, you find you "trigger" another piece of guidance, follow that chain until you reach the end.

#### When making ANY changes to content strings, localisers, or resource files:
* Make sure that you have visually confirmed that the text has been updated as you would expect when you run the site, in both English and Welsh.

#### When adding a new content string:
* If the new content string relates to brand new copy text, ensure it is not just the verbatim English text. Instead, use the format `[GeneralPurposeOfText]String` e.g. `WelcomePageTitleString` which is the new standard for resource file keys.
* Check the new content string is surrounded by an appropriate localiser. Examples:
  * `@SharedLocaliser["<ContentString>"]`
  * `@SharedLocaliser["<ContentString>"].Value`
* If you're using existing copy text in a new location, check that the new content string matches the key relating to that copy text in the resource file that the Localiser references.
* If you're adding completely new copy text that doesn't already have an entry in the resource file: ensure that you add the complete Key/English Value/Welsh Value triad to the resource file.

#### When editing a content string:
* Ensure that your newly edited content string corresponds to a resource file key within the resource file that the localiser references.
* If your edited content string is for the purpose of adding completely new copy text that doesn't already have an entry in the resource file: ensure that you add the complete Key/English Value/Welsh Value triad to the resource file.

#### When deleting a content string:
* Check in the corresponding resource file that it is still being referenced elsewhere (using find usages, or your IDE's search function). If it is not being used elsewhere, delete the key from the resource file (in Rider, use Safe Delete).

#### When adding a new key to a resource file:
* Check the key is not a duplicate (case insensitively).
* Ensure the key is not just the verbatim English text. Instead, use the format `[GeneralPurposeOfText]String` e.g. `WelcomePageTitleString` which is the new standard for resource file keys.

#### When editing a resource file key:
* Check that all references to the original key have been updated to the new key. Use Find Usages in Rider, *and* the IDE's search function to ensure you have found every usage.
* In Rider, use the [Localisation Manager's](https://www.jetbrains.com/help/rider/Resources__LocalizationManager.html) Rename tool to properly rename the key in the resource files.
* Ensure the new key follows the guidance for "adding a new key to a resource file"

#### When editing a resource file value:
* Keys may be used in multiple places, so ensure the new value is correct for all usages of the key. Make a new key if this is not the case.
* Do not leave the value empty.

#### When deleting a resource file value:
* Don't delete resource file values without replacing the text. Leaving the value blank will be considered as an empty string to display, so no text will be visible.

#### When adding a new enum/enum value:
* Check if the new enum/enum value will be displayed to the user as part of a radio button/checkbox/dropdown.
  * If you're making a new enum, add a `[Display(ResourceType = typeof(Resources.Enum.<resource filename>), Description = nameof(Resources.Enum.<resource filename>.<key>))]` attribute above each option.
    * As always, ensure the resource file (and its .cy counterpart) at the path specified `Resources.Enum.<resource filename>` and the Key/English Value/Welsh Value triads you want to reference do exist, and create them if they do not.
  * If you're adding a new value to an existing enum, add the attribute `[Display(ResourceType = typeof(Resources.Enum.<resource filename>), Description = nameof(Resources.Enum.<resource filename>.<key>))]` above your new option, add a new Key/English Value/Welsh Value triad to the enum's existing resource file.

#### When using GovDesignSystem validation:
* Check that you have referenced the error messages and translations appropriately, some examples below:
  * `[GovUkValidateRequired(ErrorMessageResourceType = typeof(<resource filename>), ErrorMessageResourceName = nameof(<resource filename>.<key>))]`
  * `[GovUkDataBindingMandatoryDecimalErrorText(nameof(<resource filename>.<key>), "The temperature", typeof(<resource filename>), "SeaPublicWebsite.Resources.<resource filename>", mustBeNumberErrorMessage: nameof(<resource filename>.<key>) )]`
  * `[Range(minimum:5.0, maximum:35.0 , ErrorMessageResourceType = typeof(<resource filename>), ErrorMessageResourceName= nameof(<resource filename>.<key>))]`