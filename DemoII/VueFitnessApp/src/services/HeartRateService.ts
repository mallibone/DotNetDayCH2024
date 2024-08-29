export default class HeartRateService {
  getHeartRate(): number {
    return Math.floor(Math.random() * (100 - 60 + 1)) + 60
  }
}