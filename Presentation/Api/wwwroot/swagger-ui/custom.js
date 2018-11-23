(function () {
  var re_leading_whitespace = /^\s+(?=[^\s])/g,
      re_is_whitespace = /^\s*$/,
      re_br = /<br ?\/?>/gi,
      re_blockquotes_for_html_fix = /^(\s*&gt;)+/gm;

  function transform_markdown(html, text) {

    var
        // leading padding string, if found
        padding = null,

        // number of non-blank lines checked - first line is usually trimmed (from the xml documentation 
        // generation), so need to check a few. Caveat here being that the second line _could_ be a list 
        // or code block, so likely needs further dev
        nonEmptyLines = 0,

        // using `html` is a bit odd here - it's already half escaped. Should be using `text`, but 
        // that is all over the place in terms of indentation.
        cleansed = html

            // revert <br/>'s to newlines - which is the default rendering in swagger-ui
            .replace(re_br, "\n")

            // fix blockquotes (markdown ">") which are escaped
            .replace(re_blockquotes_for_html_fix, function (match, p1, offset, string) {
              return match.replace(/&gt;/g, ">");
            });

    var lines = cleansed.split('\n');
    for (var i = 0, l = lines.length; i < l && nonEmptyLines < 3; ++i) {
      var line = lines[i];

      if (line && !re_is_whitespace.test(line)) {
        ++nonEmptyLines;

        var leading = re_leading_whitespace.exec(line);

        var prefix = leading && leading[0];
        if (prefix != null && prefix.length > 0 && line.length !== prefix.length) {
          padding = prefix
          break;
        }
      }
    }

    if (padding != null) {
      var padlen = padding.length;

      // remove leading padding from each line
      for (var i = 0, l = lines.length; i < l; ++i) {
        var line = lines[i];

        if (line.length > padlen && line.substring(0, padlen) === padding)
          lines[i] = line.substring(padlen);
      }

      cleansed = lines.join('\n');
    }

    return marked(cleansed);
  }

  SwaggerUi.prototype.renderGFM = function newRenderGFM(data) {
    if (data == null) {
      data = '';
    }

    $('.markdown').each(function () {
      var $node = $(this),
          markdown = transform_markdown($node.html(), $node.text());

      if ($node.is("div,td,th,section,li,a")) {

        $node.empty().html(markdown);

      } else {

        var $transformed = $("<div/>", {
          html: markdown,
          "class": "markdown"
        });
        $node.replaceWith($transformed);

      }
    });
  };
})();