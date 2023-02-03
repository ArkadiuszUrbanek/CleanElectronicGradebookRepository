import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { ChartEvent } from 'chart.js/dist/core/core.plugins'
import { Chart } from 'chart.js'

import { AttendanceStatisticalDataToSelectDTO } from './../../../../models/attendance-statistical-data-to-select'

@Component({
  selector: 'app-attendance-chart-dialog',
  templateUrl: './attendance-chart-dialog.component.html',
  styleUrls: ['./attendance-chart-dialog.component.css']
})
export class AttendanceChartDialogComponent implements OnInit {
  @ViewChild("canvas", { static: true }) canvasRef!: ElementRef<HTMLCanvasElement>

  public chart!: Chart

  constructor(public matDialogRef: MatDialogRef<AttendanceChartDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                attendanceStatisticalData: AttendanceStatisticalDataToSelectDTO[]
              }) { }

  ngOnInit(): void {
    this.initHorizontalBarChart()
  }

  private initHorizontalBarChart(): void {
    this.chart = new Chart(this.canvasRef.nativeElement.getContext("2d")!, {
      type: 'bar',
      data: {
        labels: this.data.attendanceStatisticalData.map(attendanceStatisticalData => attendanceStatisticalData.label),
        datasets: [{
            label: 'Obecności',
            data: this.data.attendanceStatisticalData.map(attendanceStatisticalData => attendanceStatisticalData.data['Present']),
            backgroundColor: "rgb(144, 226, 86)",
            borderColor: "rgb(144, 226, 86)"
          }, {
            label: 'Nieobecności',
            data: this.data.attendanceStatisticalData.map(attendanceStatisticalData => attendanceStatisticalData.data['Absent']),
            backgroundColor: "rgb(255, 82, 70)",
            borderColor: "rgb(255, 82, 70)"
          }, {
            label: 'Spóźnienia',
            data: this.data.attendanceStatisticalData.map(attendanceStatisticalData => attendanceStatisticalData.data['Late']),
            backgroundColor: "rgb(250, 153, 56)",
            borderColor: "rgb(250, 153, 56)"
          }, {
            label: 'Zwolnienia',
            data: this.data.attendanceStatisticalData.map(attendanceStatisticalData => attendanceStatisticalData.data['Excused']),
            backgroundColor: "rgb(171, 171, 171)",
            borderColor: "rgb(171, 171, 171)"
          }, {
            label: 'Nieobecności usprawiedliwione',
            data: this.data.attendanceStatisticalData.map(attendanceStatisticalData => attendanceStatisticalData.data['ExcusedAbsence']),
            backgroundColor: "rgb(248, 238, 97)",
            borderColor: "rgb(248, 238, 97)"
          }
        ]
      },
      options: {
        plugins: {
          legend: {
            onClick: (e: ChartEvent) => { e.native!.preventDefault() },
            position: 'bottom',
            labels: {
              boxWidth: 26,
              boxHeight: 26,
              font: {
                size: 12
              }
            }
          }
        },
        interaction: {
          mode: 'y'
        },
        indexAxis: 'y',
        scales: {
          x: { 
            stacked: true,
            min: 0,
          },
          y: { stacked: true }
        },
      }
    })
  }
}