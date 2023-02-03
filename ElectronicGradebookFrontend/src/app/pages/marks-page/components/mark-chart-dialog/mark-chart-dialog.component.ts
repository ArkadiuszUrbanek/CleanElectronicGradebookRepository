import { Component, OnInit, Inject, ElementRef, ViewChild } from '@angular/core'

import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog'

import { ChartEvent } from 'chart.js/dist/core/core.plugins'
import { Chart } from 'chart.js'

import { MarksStatisticalDataToSelectDTO } from '../../../../models/marks-statistical-data-to-select'

@Component({
  selector: 'app-mark-chart-dialog',
  templateUrl: './mark-chart-dialog.component.html',
  styleUrls: ['./mark-chart-dialog.component.css']
})
export class MarkChartDialogComponent implements OnInit {
  @ViewChild("canvas", { static: true }) canvasRef!: ElementRef<HTMLCanvasElement>

  public chart!: Chart

  constructor(public matDialogRef: MatDialogRef<MarkChartDialogComponent>,
              @Inject(MAT_DIALOG_DATA) public data: {
                marksStatisticalData: MarksStatisticalDataToSelectDTO[]
              }) { }

  ngOnInit(): void {
    this.initBarChart()
  }

  private initBarChart(): void {
    this.chart = new Chart(this.canvasRef.nativeElement.getContext("2d")!, {
      type: 'bar',
      data: {
        labels: this.data.marksStatisticalData.map(markStatisticalData => markStatisticalData.label),
        datasets: [{
          label: 'Średnia ucznia',
          data: this.data.marksStatisticalData.map(markStatisticalData => markStatisticalData.pupilWeightedAverage),
          backgroundColor: ['rgb(82, 202, 61)'],
          borderColor: ['rgb(82, 202, 61)']
        },
        {
          label: 'Średnia klasy',
          data: this.data.marksStatisticalData.map(markStatisticalData => markStatisticalData.classWeightedAverage),
          backgroundColor: ['rgb(231, 192, 54)'],
          borderColor: ['rgb(231, 192, 54)']
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
          },
        },
        indexAxis: 'y',
        scales: {
          x: {
            max: 6,
            min: 0,
            ticks: {
              stepSize: 1
            }
          }
        }
      }
    })
  }
}