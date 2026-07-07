<script lang="ts">
  import type { ActivityStats } from "./models/ActivityStats";
  import type { SummaryActivity } from "./models/SummaryActivity";
  import type { GraphNode } from "./models/GraphPoint";
  import type { GraphLabel } from "./models/GraphLabel";
  import { Html, LayerCake, Svg, flatten } from "layercake";
  import AxisX from "../../chartcomponents/AxisX.svelte";
  import AxisY from "../../chartcomponents/AxisY.svelte";
  import Multiline from "../../chartcomponents/Multiline.svelte";
  import SharedTooltip from "../../chartcomponents/SharedTooltip.html.svelte";
  import { formatDistance, formatElevation } from "../../Utilities/DataFormat";
  import type { DetailedGear } from "./models/DetailedGear";

  var data = <SummaryActivity[]>[];
  var shoeData = <DetailedGear[]>[];
  fetch("./getActivitiesSummary").then((r) => {
    r.json().then((j) => {
      data = j;
      data = data.reverse();
    });
  });

  fetch("./getAthleteGearSummary").then((r) => {
    r.json().then((j) => {
      shoeData = j;
    });
  });

  var points = <GraphNode[]>[];
  var labels = <GraphLabel[]>[];
  fetch("./getGraphData").then((r) => {
    r.json().then((j) => {
      points = j;
      if (points.length > 1) {
        points[0].LineColor = "#ab00d6";
        points[1].LineColor = "#007bff";
        for (let i = 0; i < 10; i++) {
          labels.push({
            Index: i,
            Distance: formatDistance(points[0].Values[i].Value * 10),
            TotalElevationGain: formatElevation(points[1].Values[i].Value),
            Week: points[0].Values[i].Week,
          });
        }
      }
    });
  });

  const formatLabelY = (d: number) => {
    return d;
  };
</script>

<div class="container my-5">
  {#if points.length == 0}
    <p>Loading graph...</p>
  {:else}
    <div class="chart-container">
      <LayerCake
        data={points}
        x="Index"
        y="Value"
        z="ValueType"
        flatData={flatten(points, "Values")}
      >
        <Svg>
          <AxisX
            format={(d) => points[0].Values[d].Week}
            snapLabels
            tickMarks
            gridlines={false}
          />
          <AxisY />
          <!-- <Line /> -->
          <Multiline />
        </Svg>
        <Html>
          <!-- <Labels /> -->
          <SharedTooltip formatTitle={formatLabelY} dataset={labels} />
        </Html>
      </LayerCake>
    </div>
  {/if}

  <h2 class="fw-bold mb-3">Last weeks activities:</h2>
  <div class="row">
    {#if data.length == 0}
      <p>Loading...</p>
    {:else}
      <div class="col-8">
        <ul class="list-group">
          {#each data as a}
            <li class="list-group-item list-group-light">
              <a
                href="/StravaActivity/Index?id={a.id}"
                class="list-group-item list-group-item-action"
              >
                <div class="flex align-items-center">
                  <div class="header">
                    <strong>{a.name}</strong> - {new Date(
                      a.start_date_local,
                    ).toDateString()}
                  </div>
                  <div class="content flex-column">
                    <div class="d-flex justify-content-between">
                      <div class="flex column">
                        {#if a.type != "Run" && a.type != "Hike" && a.type != "Walk"}
                          <div>Distance: {formatDistance(a.distance)}</div>
                        {/if}
                        <div>
                          Duration: {Math.round(a.moving_time / 60)} minutes
                        </div>
                      </div>
                      <div>
                        <div>
                          Elevation gain: {formatElevation(
                            a.total_elevation_gain,
                          )}
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </a>
            </li>
          {/each}
        </ul>
      </div>
    {/if}
    <div class="col-3">
      <h2 class="">My shoes:</h2>
      {#if shoeData.length == 0}
        <p>Loading shoe data...</p>
      {:else}
        {#each shoeData as shoe}
          <div class="card mb-3">
            <div class="card-body">
              <h5 class="card-title">{shoe.name}</h5>
              <p class="card-text">Distance: {formatDistance(shoe.distance)}</p>
            </div>
          </div>
        {/each}
      {/if}
    </div>
  </div>
  <style>
    .chart-container {
      height: 150px;
      display: flex;
      margin-bottom: 20px;
      padding: 20px 50px;
    }
    .layercake-container {
      width: 100%;
    }
    /* .line */
    .path-line {
      fill-opacity: 0;
      stroke-linejoin: round;
      stroke-linecap: round;
      stroke-width: 2;
    }
    /* .axis .tick text */
    .tick {
      font-size: 10px;
    }

    line,
    .tick line {
      stroke: #aaa;
    }

    .tick text {
      fill: #666;
    }

    .tick .tick-mark,
    .baseline {
      stroke-dasharray: 0;
    }        
    .axis.snapLabels .tick:last-child text {
      transform: translateX(55px);
    }
    .axis.snapLabels .tick.tick-0 text {
      transform: translateX(-53px);
    }
    .tooltip {
      position: absolute;
      font-size: 13px;
      pointer-events: none;
      border: 1px solid #ccc;
      background: rgba(255, 255, 255, 0.85);
      transform: translate(-50%, -100%);
      padding: 5px;
      opacity: 0.9;
      z-index: 15;
      pointer-events: none;
    }
    .toolTipLine {
      position: absolute;
      top: 0;
      bottom: 0;
      width: 1px;
      border-left: 1px dotted #666;
      pointer-events: none;
    }
    .tooltip,
    .toolTipLine {
      transition:
        left 250ms ease-out,
        top 250ms ease-out;
    }
    .title {
      font-weight: bold;
    }
    .key {
      color: #000;
    }
    .bg {
      position: absolute;
      top: 0;
      right: 0;
      bottom: 0;
      left: 0;
    }
  </style>
</div>
