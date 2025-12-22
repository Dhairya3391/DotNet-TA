type MdAstNode = {
  type?: string;
  lang?: string | null;
  children?: MdAstNode[];
};

function normalizeLang(rawLang: string): string {
  const lang = rawLang.trim().toLowerCase();

  // Common aliases used in our docs → Prism language ids
  switch (lang) {
    case "c#":
    case "csharp":
    case "cs":
      return "csharp";

    // Razor views are typically written as ```cshtml, but Prism uses `aspnet`.
    case "cshtml":
      return "aspnet";

    default:
      return lang;
  }
}

function walkAndNormalize(node: MdAstNode): void {
  if (node.type === "code" && typeof node.lang === "string" && node.lang.trim() !== "") {
    node.lang = normalizeLang(node.lang);
  }

  if (Array.isArray(node.children)) {
    for (const child of node.children) {
      walkAndNormalize(child);
    }
  }
}

export default function normalizeCodeBlockLang() {
  return (tree: MdAstNode) => {
    walkAndNormalize(tree);
  };
}
