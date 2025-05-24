# Contributing to the Supernova Mod

## Index

* [Pull requests](#pull-requests)
* [Filing bugs](#filing-bugs)
* [Feature requests](#feature-requests)
* [Technical resources](#technical-resources)
* [Getting help](#getting-help)
* [Supernova Mod source code directory structure](#supernova-mod-source-code-directory-structure)
* [Git commit rules](#git-commit-rules)

## Pull requests

We welcome pull requests to improve localization, fix bugs and to implement RFCs

By submitting a pull request, you certify that you have the necessary rights to submit the work, that the work does not violate any third-party rights, and that you license your contribution under the GNU General Public License (see [LICENSE](./LICENSE)).

Pull requests should be submitted against the `development` branch.

If your pull request exhibits conflicts with the base branch, please resolve them by using git rebase instead of git merge.


## Filing bugs

Bugs can be filed on [GitHub Issues](https://github.com/php/php-src/issues/new/choose) or at the #support-form channel on our Discord server.
If this is the first time you've filed a bug, we suggest reading the [guide to reporting a bug](https://bugs.php.net/how-to-report.php).

Where possible, please include a self-contained reproduction case!


## Feature requests

Feature requests are generally filed on [GitHub Issues](https://github.com/php/php-src/issues/new/choose) or at the #support-form channel on our Discord server.

## Technical resources

 * [tModLoader guide for developers](https://github.com/tModLoader/tModLoader/wiki/tModLoader-guide-for-developers)
 TBA

## Getting help

If you are having trouble contributing, or just want to talk to a human
about what you're working on, you can contact us via Discord.

## Supernova Mod source code directory structure

```bash
<Supernova Mod>/
 ├─ .git/                           # Git configuration and source directory
 ├─ Assets/                         # None item specific Assets (like images, shaders, etc.)
 ├─ Common/                         # Supernova Mod systems
 ├─ Content/                        # Contains all items, npcs, etc. added to the Supernova Mod
 ├─ Core/                           # Contains the Supernova Mod Core SDK (helper classes)
 ├─ Localization/                   # Text libraries for English and translations of the English version 
 └─ ...
```

## Git commit rules

This section refers to contributors that have Git push access and make commit
changes themselves. We'll assume you're basically familiar with Git, but feel
free to post your questions on the mailing list. Please have a look at the more
detailed [information on Git](https://git-scm.com/).

1. If you don't know how to do something, ask first!

2. Test your changes before committing them.

3. Work in a separate branch and create a pull request to [development] for it be accepted.

TBA

Thank you for contributing to the Supernova Mod!