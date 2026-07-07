<script lang="ts">
    import Modal from "../../sharedComponents/Modal.svelte";
    import type { PlannerItem } from "./models/PlannerItem";
    
    let { showModal = $bindable(), date = $bindable(), plan }: { showModal: boolean; date: string; plan: PlannerItem | null } = $props();
    let description = $state(plan?.Description || "");
    let distance = $state(0);
    let elevation = $state(0);
            
    const savePlan = async () => {                
        var body = JSON.stringify({
            planDate: date,
            Description: description,
            Distance: distance,
            Elevation: elevation,
        });
        fetch("./SavePlan", {
        method: "post",        
        body: body,
        headers: { "Content-Type": "application/json" },
    }).then((res) => res.json().then((data) => {
        console.log("Plan saved:", data);     
        showModal = false; // Close the modal after saving
    }).catch((err)=>{
        console.error("Error parsing response:", err);
    })).catch((err) => {
        console.error("Error saving plan:", err);
    })};
</script>

<Modal bind:showModal>
    {#snippet header()}
        <h2>
            Planner Item Details for {date}
        </h2>
    {/snippet}
    <div class="p-3">        
        <span>{plan?.Description}</span>        
        <input
            type="text"
            placeholder="Description"
            class="form-control mb-3"
            bind:value={description}
        />
        <input
            type="number"
            placeholder="Distance (km)"
            class="form-control mb-3"
            bind:value={distance}
        />
        <input
            type="number"
            placeholder="Elevation (m)"
            class="form-control mb-3"
            bind:value={elevation}
        />
        <button class="btn btn-primary" onclick={savePlan}>
            Save
        </button>
    </div>
</Modal>

<style>
    input::-webkit-outer-spin-button,
    input::-webkit-inner-spin-button {
        /* display: none; <- Crashes Chrome on hover */
        -webkit-appearance: none;
        margin: 0; /* <-- Apparently some margin are still there even though it's hidden */
    }

    input[type="number"] {
        -moz-appearance: textfield; /* Firefox */
    }
</style>
