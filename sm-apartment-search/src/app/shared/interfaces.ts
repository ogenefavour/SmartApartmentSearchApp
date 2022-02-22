export interface MarketList {
        market: string;
}


export interface SearchResultContents {
    name: string;
    market: string;
    state: string;
    isManagement: boolean;
}

export interface SearchResult {
    data: SearchResultContents[];
}
