import { defineConfig } from "vite";

export default defineConfig({
  build: {
    sourcemap: true,
  },
  publicDir: "public",
  resolve: {
    alias: {
      "@": "/src", // Adjust this based on your source directory
    },
  },
  server: {
    port: 3000,
    open: true,
  },
});
