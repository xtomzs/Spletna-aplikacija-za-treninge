import { mount } from 'svelte'
import Activity from './Activity.svelte'
const app = mount(Activity, {
    target: document.getElementById('app')!    
})

export default app
