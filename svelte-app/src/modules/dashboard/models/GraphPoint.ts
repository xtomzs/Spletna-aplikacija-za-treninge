export interface GraphPoint {
    Index: string;
    Week: string;
    Value: number;
}
export interface GraphNode {
    ValueType: string
    Values: GraphPoint[]
    LineColor: string
}