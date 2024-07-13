export class Match {
    constructor(
        public id: number,
        public expiryTimestamp: string,
        public startDate: Date,
        public endDate: Date,
        public isDone: boolean,
        public winnerId?: number,
        public winnerName?: string,
        public currentUserScore: number = 0,
    ) { }
}