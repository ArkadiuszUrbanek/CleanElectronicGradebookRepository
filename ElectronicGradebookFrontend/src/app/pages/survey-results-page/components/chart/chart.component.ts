import { Component, OnInit, ViewChild, ElementRef, Input, OnChanges } from '@angular/core'

import { Chart, registerables } from 'chart.js'
import { ChartEvent } from 'chart.js/dist/core/core.plugins'
import { LegendElement, LegendItem, TooltipItem } from 'chart.js/dist/types/index'
import { interpolateGreens } from 'd3-scale-chromatic'

Chart.register(...registerables)

@Component({
  selector: 'app-chart',
  templateUrl: './chart.component.html',
  styleUrls: ['./chart.component.css']
})
export class ChartComponent implements OnInit, OnChanges {
  @ViewChild("canvas", { static: true }) canvasRef!: ElementRef<HTMLCanvasElement>
  @Input() type!: 'pie' | 'bar'
  @Input() labels!: string[]
  @Input() data!: number[]
  @Input() max!: number

  public chart!: Chart
  private isInitiallyLoading: boolean = true

  constructor() { }

  public ngOnChanges(): void {
    if (this.isDataMissing()) return

    if(!this.isInitiallyLoading) {
      this.chart.destroy()
      if (this.type === 'pie') this.initPieChart()
      if (this.type === 'bar') this.initBarChart()
    }
  }

  public ngOnInit(): void {
    if (this.isDataMissing()) return

    if (this.type === 'pie') this.initPieChart()
    if (this.type === 'bar') this.initBarChart()
    this.isInitiallyLoading = false
  }

  public isDataMissing(): boolean {
    return this.data.reduce((partialSum, a) => partialSum + a, 0) == 0
  }

  private calculatePoint(i: number, intervalSize: number, colorRangeInfo: any) {
    var { colorStart, colorEnd, useEndAsStart } = colorRangeInfo;
    return (useEndAsStart
      ? (colorEnd - (i * intervalSize))
      : (colorStart + (i * intervalSize)));
  }

  private interpolateColors(dataLength: number, colorScale: (t: number) => string, colorRangeInfo: ColorRangeInfo) {
    var { colorStart, colorEnd } = colorRangeInfo;
    var colorRange = colorEnd - colorStart;
    var intervalSize = colorRange / dataLength;
    var i, colorPoint;
    var colorArray = [];
  
    for (i = 0; i < dataLength; i++) {
      colorPoint = this.calculatePoint(i, intervalSize, colorRangeInfo);
      colorArray.push(colorScale(colorPoint));
    }
  
    return colorArray;
  }  

  private initPieChart(): void {
    this.chart = new Chart(this.canvasRef.nativeElement.getContext("2d")!, {
      type: this.type,
      data: {
        labels: this.labels,
        datasets: [{
          label: '',
          data: this.data,
          backgroundColor: this.interpolateColors(this.data.length, interpolateGreens, { 
            colorStart: 0.1,
            colorEnd: 0.9,
            useEndAsStart: false
          }),
          hoverOffset: 4
        }]
      },
      options: {
        maintainAspectRatio: false,
        animation: {
          duration: 1500,
        },
        responsive: true,
        layout: {
          padding: 0
        },
        plugins: {
          legend: {
            display: true,
            position: 'left',
            onClick: (e: ChartEvent) => { e.native!.preventDefault() },
            onHover: (e: ChartEvent, legendItem: LegendItem, legend: LegendElement<"pie">) => { 
              const chart = legend.chart
              const tooltip = chart.tooltip

              const chartArea = chart.chartArea;
              tooltip!.setActiveElements([{
                datasetIndex: 0,
                index: legendItem.index!,
              }], {
                x: (chartArea.left + chartArea.right) / 2,
                y: (chartArea.top + chartArea.bottom) / 2,
              })

              chart.setActiveElements([{
                datasetIndex: 0,
                index: legendItem.index!,
              }])

              chart.update()
            },
            onLeave: (e: ChartEvent, legendItem: LegendItem, legend: LegendElement<"pie">) => {
              const chart = legend.chart 
              const tooltip = chart.tooltip
              if (tooltip!.getActiveElements().length > 0) tooltip!.setActiveElements([], {x: 0, y: 0})
              if (chart.getActiveElements().length > 0) chart.setActiveElements([])

              chart.update()
            },
            labels: {
              usePointStyle: true,
              pointStyle: 'circle'
            }
          },
          tooltip: {
            callbacks: {
              title: () => '',
              label: function(tooltipItem: TooltipItem<"pie">) {
                var percent = Math.round((tooltipItem.dataset.data[tooltipItem.dataIndex] / tooltipItem.dataset.data.reduce((partialSum, a) => partialSum + a, 0)) * 100).toFixed(2)
                return ` ${tooltipItem.label}: ${tooltipItem.dataset.data[tooltipItem.dataIndex]} (${percent}%)`
              }
            }
          }
        }
      }
    })
  }

  private initBarChart(): void {
    this.chart = new Chart(this.canvasRef.nativeElement.getContext("2d")!, {
      type: this.type,
      data: {
        labels: this.labels,
        datasets: [{
          label: '',
          data: this.data,
          backgroundColor: this.interpolateColors(this.data.length, interpolateGreens, { 
            colorStart: 0.1,
            colorEnd: 0.9,
            useEndAsStart: false
          }),
        }]
      },
      options: {
        indexAxis: 'x',
        maintainAspectRatio: false,
        animation: {
          duration: 1500,
        },
        responsive: true,
        layout: {
          padding: 0
        },
        scales: {
          y: {
            min: 0,
            max: this.max + 5 - this.max % 5,
            ticks: {
              stepSize: 5
            }
          }
        },
        plugins: {
          legend: {
            display: false,
          },
          tooltip: {
            callbacks: {
              title: () => '',
              label: function(tooltipItem: TooltipItem<"pie">) {
                var percent = Math.round((tooltipItem.dataset.data[tooltipItem.dataIndex] / tooltipItem.dataset.data.reduce((partialSum, a) => partialSum + a, 0)) * 100).toFixed(2)
                return ` ${tooltipItem.label}: ${tooltipItem.dataset.data[tooltipItem.dataIndex]} (${percent}%)`
              }
            }
          }
        }
      }
    })
  }
}

interface ColorRangeInfo {
  colorStart: number,
  colorEnd: number,
  useEndAsStart: boolean,
}
