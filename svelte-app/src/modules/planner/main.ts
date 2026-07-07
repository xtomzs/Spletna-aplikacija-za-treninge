import { mount } from 'svelte'
import Planner from './Planner.svelte'

const app = mount(Planner, {
  target: document.getElementById('app')!
})

export default app
