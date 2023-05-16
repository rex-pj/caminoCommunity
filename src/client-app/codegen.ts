import { CodegenConfig } from "@graphql-codegen/cli";
import { apiConfig } from "./src/config/api-config";

const config: CodegenConfig = {
  schema: apiConfig.camino_graphql,
  documents: ["src/**/*.ts", "src/**/*.tsx"],
  generates: {
    "./src/__generated__/": {
      preset: "client",
      plugins: [],
      presetConfig: {
        gqlTagName: "gql",
      },
    },
  },
  ignoreNoDocuments: true,
};

export default config;
