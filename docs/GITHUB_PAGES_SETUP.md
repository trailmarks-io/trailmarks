# GitHub Pages Setup Guide

This guide explains how to enable GitHub Pages for the Trailmarks documentation.

## Prerequisites

- Repository admin access
- Documentation has been merged to the `main` branch

## Setup Steps

### 1. Enable GitHub Pages

1. Navigate to your repository on GitHub: https://github.com/trailmarks-io/trailmarks

2. Click on **Settings** (gear icon in the top navigation)

3. In the left sidebar, click on **Pages** (under "Code and automation")

4. Under **Build and deployment**:
   - **Source**: Select `GitHub Actions`
   - No need to select a branch or folder

5. Click **Save** (if required)

### 2. Verify Workflow Permissions

1. In **Settings**, go to **Actions** → **General**

2. Scroll down to **Workflow permissions**

3. Ensure the following is selected:
   - ✅ **Read and write permissions**
   - ✅ **Allow GitHub Actions to create and approve pull requests**

4. Click **Save** if you made changes

### 3. Trigger Documentation Build

The documentation will automatically build when:

- You push to the `main` branch
- You push to the `develop` branch (for preview)
- Files in the `docs/` directory change
- The workflow file `.github/workflows/docs.yml` is modified

To manually trigger a build:

1. Go to **Actions** tab
2. Select **Documentation** workflow
3. Click **Run workflow**
4. Select the branch (usually `main`)
5. Click **Run workflow**

### 4. Monitor Build Progress

1. Go to **Actions** tab
2. Click on the latest **Documentation** workflow run
3. Watch the build progress:
   - ✅ **Build** - Converts AsciiDoc to HTML, processes PlantUML diagrams
   - ✅ **Deploy** - Publishes to GitHub Pages (only on `main` branch)

### 5. Access Documentation

Once deployed, your documentation will be available at:

```
https://trailmarks-io.github.io/trailmarks/
```

Or check the **Actions** workflow output for the exact URL.

## Troubleshooting

### Build Fails

If the build fails:

1. Check the **Actions** tab for error messages
2. Review the workflow logs
3. Common issues:
   - Syntax errors in AsciiDoc files
   - PlantUML diagram errors
   - Missing dependencies

### Pages Not Showing

If the deployment succeeds but pages don't show:

1. Wait a few minutes (first deployment can take 5-10 minutes)
2. Clear your browser cache
3. Check the repository **Settings** → **Pages** for the published URL
4. Verify the workflow completed the **deploy** job

### Diagrams Not Rendering

If diagrams don't appear:

1. Check the workflow logs for PlantUML errors
2. Verify PlantUML syntax in the `.adoc` files
3. Ensure Java is available in the workflow (it should be by default)

## Custom Domain (Optional)

To use a custom domain:

1. In **Settings** → **Pages**
2. Under **Custom domain**, enter your domain (e.g., `docs.trailmarks.io`)
3. Follow the instructions to configure DNS
4. Enable **Enforce HTTPS** (recommended)

## Workflow Configuration

The documentation workflow is configured in:

```
.github/workflows/docs.yml
```

Key features:

- **Triggers**: Push to `main`/`develop`, manual dispatch
- **Tools**: AsciiDoctor, asciidoctor-diagram, PlantUML, Rouge
- **Output**: Static HTML site
- **Deployment**: GitHub Pages (main branch only)

## Updating Documentation

After setup, simply:

1. Edit `.adoc` files in the `docs/` directory
2. Commit and push to your branch
3. Create a pull request to `main`
4. Once merged, documentation automatically rebuilds and deploys

## Monitoring

- **Build Status**: Check the Actions tab
- **Deployment Status**: Check Settings → Pages
- **Site URL**: Listed in Settings → Pages after first deployment

## Additional Resources

- [GitHub Pages Documentation](https://docs.github.com/en/pages)
- [GitHub Actions Documentation](https://docs.github.com/en/actions)
- [AsciiDoctor Documentation](https://asciidoctor.org/docs/)

## Support

If you encounter issues:

1. Check this guide
2. Review workflow logs in Actions tab
3. Check GitHub Pages status: https://www.githubstatus.com/
4. Open an issue in the repository

---

**Note**: The first deployment may take up to 10 minutes. Subsequent deployments are usually faster (2-5 minutes).
