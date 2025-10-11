#!/bin/bash
# Script to build documentation locally

set -e

echo "🔨 Building Trailmarks Documentation"
echo "======================================"

# Check if asciidoctor is installed
if ! command -v asciidoctor &> /dev/null; then
    echo "❌ Error: asciidoctor is not installed"
    echo "Please install it with: gem install asciidoctor asciidoctor-diagram rouge"
    exit 1
fi

# Create output directory
OUTPUT_DIR="_site"
echo "📁 Creating output directory: ${OUTPUT_DIR}"
mkdir -p ${OUTPUT_DIR}

# Set common attributes
ATTRS="-a source-highlighter=rouge -a icons=font -a toc=left -a toclevels=3 -a sectanchors -a sectlinks"

# Function to convert a document
convert_doc() {
    local input=$1
    local output_dir=$2
    local doc_name=$(basename $(dirname $input))
    
    echo "📄 Converting: $input -> ${output_dir}/"
    asciidoctor -r asciidoctor-diagram ${ATTRS} -D ${output_dir} ${input}
}

# Convert main index
echo ""
echo "Converting main documentation index..."
convert_doc "docs/index.adoc" "${OUTPUT_DIR}"

# Convert architecture docs
echo ""
echo "Converting architecture documentation..."
mkdir -p ${OUTPUT_DIR}/architecture
convert_doc "docs/architecture/index.adoc" "${OUTPUT_DIR}/architecture"

# Convert user guide
echo ""
echo "Converting user guide..."
mkdir -p ${OUTPUT_DIR}/user-guide
convert_doc "docs/user-guide/index.adoc" "${OUTPUT_DIR}/user-guide"

# Convert admin guide
echo ""
echo "Converting admin guide..."
mkdir -p ${OUTPUT_DIR}/admin-guide
convert_doc "docs/admin-guide/index.adoc" "${OUTPUT_DIR}/admin-guide"

# Copy diagram images if they exist
if [ -d ".asciidoctor" ]; then
    echo ""
    echo "📊 Copying diagram images..."
    mkdir -p ${OUTPUT_DIR}/images
    find .asciidoctor -type f \( -name "*.png" -o -name "*.svg" \) -exec cp {} ${OUTPUT_DIR}/images/ \;
fi

echo ""
echo "✅ Documentation built successfully!"
echo "📂 Output directory: ${OUTPUT_DIR}"
echo ""
echo "To view the documentation, open: ${OUTPUT_DIR}/index.html"
echo "Or run: open ${OUTPUT_DIR}/index.html (macOS) or xdg-open ${OUTPUT_DIR}/index.html (Linux)"
