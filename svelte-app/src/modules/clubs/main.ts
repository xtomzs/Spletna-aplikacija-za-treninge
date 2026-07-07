import { mount } from 'svelte'
import ClubsDashboard from './ClubsDashboard.svelte'
const app = mount(ClubsDashboard, {
    target: document.getElementById('app')!    
})

export default app;