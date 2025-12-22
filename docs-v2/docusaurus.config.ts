import { themes as prismThemes } from "prism-react-renderer";
import type { Config } from "@docusaurus/types";
import type * as Preset from "@docusaurus/preset-classic";
import normalizeCodeBlockLang from "./src/remark/normalizeCodeBlockLang";

// This runs in Node.js - Don't use client-side code here (browser APIs, JSX...)

const config: Config = {
  title: "DotNetTA — .NET Labs & Training",
  tagline: "Hands-on .NET labs (C#, ASP.NET, SQL) with clear steps and practice tasks.",
  favicon: "img/favicon.ico",

  // Future flags, see https://docusaurus.io/docs/api/docusaurus-config#future
  future: {
    v4: true, // Improve compatibility with the upcoming Docusaurus v4
  },

  url: "https://dotnet.noobokay.me",
  baseUrl: "/",

  onBrokenLinks: "throw",

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: "en",
    locales: ["en"],
  },

  presets: [
    [
      "classic",
      {
        docs: {
          sidebarPath: "./sidebars.ts",
          remarkPlugins: [normalizeCodeBlockLang],
        },
        theme: {
          customCss: "./src/css/custom.css",
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    // Replace with your project's social card
    image: "img/docusaurus-social-card.jpg",
    metadata: [
      {
        name: "description",
        content:
          "DotNetTA provides structured .NET training labs covering C#, ASP.NET MVC/Core concepts, Razor views, and SQL, with practical examples and mini tasks.",
      },
      {
        name: "keywords",
        content:
          "DotNetTA,.NET,C#,CSharp,ASP.NET,ASP.NET Core,MVC,Razor,CSHTML,SQL,ADO.NET,Entity Framework,Lab,Tutorial,Training",
      },
      { property: "og:site_name", content: "DotNetTA" },
      { name: "twitter:card", content: "summary_large_image" },
    ],
    colorMode: {
      respectPrefersColorScheme: true,
    },
    navbar: {
      title: "DotNetTA",
      logo: {
        alt: "DotNetTA Logo",
        src: "img/logo.svg",
      },
      items: [
        {
          type: "docSidebar",
          sidebarId: "labsSidebar",
          position: "left",
          label: "Labs",
        },
      ],
    },
    footer: {
      style: "dark",
      copyright: `Copyright © ${new Date().getFullYear()} DotNetTA. Built by <a href="https://github.com/Dhairya3391" target="_blank" rel="noopener noreferrer">Dhairya</a> and <a href="https://github.com/harpalll" target="_blank" rel="noopener noreferrer">Harpal</a>.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
      additionalLanguages: ["csharp", "aspnet", "bash", "sql"],
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
