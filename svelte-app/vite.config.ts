import { defineConfig } from 'vite'
import { svelte } from '@sveltejs/vite-plugin-svelte'

// https://vite.dev/config/
export default defineConfig({
  build:{
    rollupOptions: {
      input:{
        'main': './src/main.ts',
        'dashboard': './src/modules/dashboard/main.ts',
        'activity': './src/modules/activity/main.ts',
        'clubsdashboard': './src/modules/clubs/main.ts',
        'planner': './src/modules/planner/main.ts',
      },
      output:{
        entryFileNames: '[name].js',
        assetFileNames: '[name].[ext]'
      }
    },
    outDir: '../wwwroot/',
    emptyOutDir: true
  },
  plugins: [svelte()],
})
