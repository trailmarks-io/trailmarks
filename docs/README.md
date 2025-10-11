# Trailmarks Documentation

This directory contains the complete documentation for the Trailmarks application.

## Structure

The documentation is organized into three main sections:

- **[architecture/](architecture/)** - Technical architecture documentation following the ARC42 template
- **[user-guide/](user-guide/)** - Documentation for end users
- **[admin-guide/](admin-guide/)** - Documentation for system administrators and content moderators

## Format

All documentation is written in **AsciiDoc** format (`.adoc` files) with **PlantUML** for diagrams.

### Why AsciiDoc?

- More powerful than Markdown for technical documentation
- Better support for complex document structures
- Native diagram support with PlantUML
- Can be converted to multiple output formats (HTML, PDF, etc.)

### Viewing Documentation

#### Option 1: GitHub Pages (Recommended)

The documentation is automatically built and published to GitHub Pages on every commit to `main`:

- **GitHub Pages URL**: https://trailmarks-io.github.io/trailmarks/

#### Option 2: Local HTML Conversion

Install AsciiDoctor and convert to HTML:

```bash
# Install AsciiDoctor (requires Ruby)
gem install asciidoctor
gem install asciidoctor-diagram

# Convert to HTML
asciidoctor -r asciidoctor-diagram docs/index.adoc

# Open in browser
open docs/index.html
```

#### Option 3: IDE/Editor Plugins

Most modern IDEs have AsciiDoc preview plugins:

- **VS Code**: [AsciiDoc extension](https://marketplace.visualstudio.com/items?itemName=asciidoctor.asciidoctor-vscode)
- **IntelliJ IDEA**: Built-in AsciiDoc support
- **Atom**: [AsciiDoc Preview package](https://atom.io/packages/asciidoc-preview)

## Diagrams

Diagrams are created using PlantUML with the C4 model for architecture diagrams.

### PlantUML Syntax

Diagrams are embedded directly in AsciiDoc files:

```asciidoc
[plantuml,diagram-name,svg]
----
@startuml
!include https://raw.githubusercontent.com/plantuml-stdlib/C4-PlantUML/master/C4_Context.puml

Person(user, "User", "Application user")
System(app, "Trailmarks", "Web application")

Rel(user, app, "Uses")
@enduml
----
```

### C4 Model

Architecture diagrams use the C4 model:

- **Context**: System context and external dependencies
- **Container**: High-level technical building blocks
- **Component**: Component-level details
- **Code**: Class diagrams (when needed)

## Editing Documentation

### General Guidelines

1. **Use clear, concise language**
2. **Include code examples where appropriate**
3. **Keep diagrams up-to-date with the code**
4. **Use consistent terminology**
5. **Add cross-references between documents**

### AsciiDoc Syntax Basics

```asciidoc
= Document Title
:toc: left
:icons: font

== Section Heading

=== Subsection

Regular paragraph text.

* Bullet list item
* Another item

. Numbered list
. Another item

[source,bash]
----
code block
----

[NOTE]
====
Admonition block for notes
====
```

### PlantUML Guidelines

1. **Use C4 model** for architecture diagrams
2. **Keep diagrams simple** - don't try to show everything
3. **Use consistent naming** across diagrams
4. **Include legends** when needed
5. **Export as SVG** for better quality

## Building Documentation

### Local Build

```bash
# Install dependencies
gem install asciidoctor asciidoctor-diagram rouge

# Build all documentation
./scripts/build-docs.sh
```

### CI/CD Pipeline

Documentation is automatically built and published via GitHub Actions:

- **Trigger**: Push to `main` or `develop` branches
- **Workflow**: `.github/workflows/docs.yml`
- **Output**: GitHub Pages

## Contributing

When updating documentation:

1. Edit the appropriate `.adoc` file
2. Test locally with AsciiDoctor preview
3. Commit with conventional commit message: `docs: description`
4. Create pull request
5. Documentation will be automatically built and published

## Resources

### AsciiDoc

- [AsciiDoc Syntax Quick Reference](https://docs.asciidoctor.org/asciidoc/latest/syntax-quick-reference/)
- [AsciiDoctor User Manual](https://docs.asciidoctor.org/asciidoc/latest/)
- [AsciiDoc Writer's Guide](https://asciidoctor.org/docs/asciidoc-writers-guide/)

### PlantUML

- [PlantUML Documentation](https://plantuml.com/)
- [C4-PlantUML](https://github.com/plantuml-stdlib/C4-PlantUML)
- [PlantUML Language Reference](https://plantuml.com/guide)

### ARC42

- [ARC42 Template](https://arc42.org/overview)
- [ARC42 Documentation](https://docs.arc42.org/home/)
- [ARC42 Examples](https://arc42.org/examples)
