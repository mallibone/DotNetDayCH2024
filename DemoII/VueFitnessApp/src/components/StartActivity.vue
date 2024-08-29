<script setup lang="ts">
import { ref, onMounted } from 'vue'
import { useStore } from '../Store'
import HeartRateService from '../services/HeartRateService'
import { Chart, registerables } from 'chart.js'

Chart.register(...registerables)

const store = useStore()
const isTracking = ref(false)
const heartRate = ref(0)
const duration = ref(0)
const heartRates = ref<number[]>([])
const maxHeartRate = ref(0)
const minHeartRate = ref(Infinity)
const avgHeartRate = ref(0)
const heartRateService = new HeartRateService()
let interval: number | undefined
let chart: Chart | undefined

const startActivity = () => {
  isTracking.value = true
  const activity = store.startActivity()
  duration.value = 0
  heartRates.value = []
  maxHeartRate.value = 0
  minHeartRate.value = Infinity
  avgHeartRate.value = 0

  interval = setInterval(() => {
    if (!isTracking.value) {
      clearInterval(interval)
      store.endActivity(activity)
      return
    }
    heartRate.value = heartRateService.getHeartRate()
    heartRates.value.push(heartRate.value)
    duration.value += 1
    maxHeartRate.value = Math.max(...heartRates.value)
    minHeartRate.value = Math.min(...heartRates.value)
    avgHeartRate.value = heartRates.value.reduce((a, b) => a + b, 0) / heartRates.value.length
    store.addActivityData(activity, heartRate.value)

    if (chart) {
      chart.data.labels?.push(duration.value)
      chart.data.datasets[0].data.push(heartRate.value)
      chart.update()
    }
  }, 1000)
}

const stopActivity = () => {
  isTracking.value = false
  const activity = store.state.activities[store.state.activities.length - 1]
  store.endActivity(activity)
}

onMounted(() => {
  const ctx = document.getElementById('heartRateChart') as HTMLCanvasElement
  chart = new Chart(ctx, {
    type: 'line',
    data: {
      labels: [],
      datasets: [{
        label: 'Heart Rate',
        data: [],
        borderColor: 'rgba(75, 192, 192, 1)',
        borderWidth: 1,
        fill: false
      }]
    },
    options: {
      scales: {
        x: {
          title: {
            display: true,
            text: 'Time (s)'
          }
        },
        y: {
          title: {
            display: true,
            text: 'Heart Rate (bpm)'
          }
        }
      }
    }
  })
})
</script>

<template>
  <div>
    <button @click="startActivity" v-if="!isTracking">Start Activity</button>
    <button @click="stopActivity" v-if="isTracking">Stop Activity</button>
    <div v-if="isTracking">
      <p>
        Current Heart Rate: {{ heartRate }} 
        <span class="heart">❤️</span>
      </p>
      <p>Duration: {{ duration }} seconds</p>
      <canvas id="heartRateChart"></canvas>
    </div>
  </div>
</template>

<style scoped>
.heart {
  display: inline-block;
  animation: blink 1s infinite;
}

@keyframes blink {
  0%, 100% { opacity: 1; }
  50% { opacity: 0; }
}
</style>