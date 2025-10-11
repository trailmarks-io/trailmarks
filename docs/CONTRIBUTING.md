# Contributing to Trailmarks Documentation

Thank you for your interest in contributing to the Trailmarks documentation!

## Documentation Structure

```
docs/
├── index.adoc              # Main documentation index
├── architecture/           # Technical architecture (ARC42)
│   └── index.adoc
├── user-guide/            # End user documentation
│   └── index.adoc
├── admin-guide/           # Administrator documentation
│   └── index.adoc
└── README.md              # This file
```

## Format

All documentation is written in **AsciiDoc** format with **PlantUML** diagrams.

### Why AsciiDoc?

- More powerful than Markdown for technical documentation
- Better table support and document structure
- Native diagram support with PlantUML
- Can be converted to multiple formats (HTML, PDF, DocBook)

## Prerequisites

To work with the documentation locally, install:

```bash
# Install Ruby (if not already installed)
# On macOS with Homebrew:
brew install ruby

# On Ubuntu/Debian:
sudo apt-get install ruby-full

# Install AsciiDoctor and extensions
gem install asciidoctor
gem install asciidoctor-diagram
gem install rouge
```

For PlantUML diagrams:

```bash
# On macOS:
brew install graphviz

# On Ubuntu/Debian:
sudo apt-get install graphviz default-jre
```

## Building Documentation Locally

### Quick Build

Use the provided build script:

```bash
./scripts/build-docs.sh
```

This will create HTML files in the `_site/` directory.

### Manual Build

Convert individual files:

```bash
# Convert main index
asciidoctor -r asciidoctor-diagram docs/index.adoc

# Convert architecture docs
asciidoctor -r asciidoctor-diagram docs/architecture/index.adoc

# Convert user guide
asciidoctor -r asciidoctor-diagram docs/user-guide/index.adoc

# Convert admin guide
asciidoctor -r asciidoctor-diagram docs/admin-guide/index.adoc
```

### Live Preview

Many editors have AsciiDoc preview:

- **VS Code**: Install "AsciiDoc" extension
- **IntelliJ IDEA**: Built-in AsciiDoc support
- **Atom**: Install "asciidoc-preview" package

## Writing Documentation

### AsciiDoc Basics

```asciidoc
= Document Title
:toc: left
:icons: font

== Level 1 Heading

=== Level 2 Heading

==== Level 3 Heading

Regular paragraph text.

*bold text*
_italic text_
`monospace text`

* Unordered list item
* Another item

. Ordered list item
. Another item

[source,bash]
----
code block
----

[NOTE]
====
This is a note admonition block.
====

[WARNING]
====
This is a warning admonition block.
====

[TIP]
====
This is a tip admonition block.
====

link:other-doc.html[Link text]
```

### Adding Diagrams

#### PlantUML Diagrams

Embed PlantUML diagrams directly:

```asciidoc
[plantuml,diagram-name,svg]
----
@startuml
actor User
User -> System : Do something
System -> Database : Query data
Database --> System : Return data
System --> User : Show result
@enduml
----
```

#### C4 Model Diagrams

For architecture diagrams, use the C4 model:

```asciidoc
[plantuml,context-diagram,svg]
----
@startuml
!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Context.puml

Person(user, "User", "Application user")
System(app, "Trailmarks", "Hiking stones application")
SystemDb(db, "Database", "PostgreSQL")

Rel(user, app, "Uses", "HTTPS")
Rel(app, db, "Reads/Writes", "SQL")
@enduml
----
```

### Tables

AsciiDoc has excellent table support:

```asciidoc
[cols="1,2,3"]
|===
|Column 1 |Column 2 |Column 3

|Cell 1.1
|Cell 1.2
|Cell 1.3

|Cell 2.1
|Cell 2.2
|Cell 2.3
|===
```

### Cross-References

Link between documents:

```asciidoc
See the link:architecture/index.html[Architecture Documentation] for details.
```

## Style Guidelines

1. **Use clear, concise language**
2. **Write in present tense**
3. **Use active voice when possible**
4. **Include code examples where appropriate**
5. **Keep diagrams simple and focused**
6. **Use consistent terminology throughout**
7. **Add cross-references between related sections**

## Documentation Sections

### Architecture Documentation

- Follow the ARC42 template structure
- Include C4 diagrams (Context, Container, Component)
- Document architectural decisions (ADRs)
- Keep technical depth appropriate for architects and developers

### User Guide

- Write for non-technical users
- Include step-by-step instructions
- Add screenshots where helpful
- Include troubleshooting section
- Write FAQs

### Admin Guide

- Write for technical administrators
- Include installation and configuration details
- Document maintenance procedures
- Include command-line examples
- Add troubleshooting for common issues

## Commit Guidelines

Use conventional commit messages:

```
docs: add deployment section to admin guide
docs: update architecture diagrams
docs: fix typo in user guide
```

## Testing Changes

Before submitting:

1. **Build locally** to check for errors:
   ```bash
   ./scripts/build-docs.sh
   ```

2. **Review generated HTML** in `_site/` directory

3. **Check diagrams** render correctly

4. **Verify links** work correctly

5. **Test on different screen sizes** (if applicable)

## Submitting Changes

1. Create a feature branch:
   ```bash
   git checkout -b docs/describe-your-changes
   ```

2. Make your changes

3. Build and test locally

4. Commit with conventional commit message:
   ```bash
   git commit -m "docs: describe your changes"
   ```

5. Push and create pull request:
   ```bash
   git push origin docs/describe-your-changes
   ```

## GitHub Actions

Documentation is automatically built and published:

- **Trigger**: Push to `main` or `develop`
- **Workflow**: `.github/workflows/docs.yml`
- **Output**: GitHub Pages at https://trailmarks-io.github.io/trailmarks/

## Resources

### AsciiDoc

- [AsciiDoc Syntax Quick Reference](https://docs.asciidoctor.org/asciidoc/latest/syntax-quick-reference/)
- [AsciiDoctor User Manual](https://docs.asciidoctor.org/asciidoc/latest/)
- [AsciiDoc Best Practices](https://asciidoctor.org/docs/asciidoc-recommended-practices/)

### PlantUML

- [PlantUML Documentation](https://plantuml.com/)
- [C4-PlantUML](https://github.com/plantuml-stdlib/C4-PlantUML)
- [PlantUML Examples](https://real-world-plantuml.com/)

### ARC42

- [ARC42 Template](https://arc42.org/overview)
- [ARC42 Documentation](https://docs.arc42.org/home/)
- [ARC42 Examples](https://arc42.org/examples)

## Questions?

If you have questions about contributing to the documentation:

1. Check existing documentation
2. Review this contributing guide
3. Open an issue on GitHub
4. Contact the maintainers

Thank you for contributing!
