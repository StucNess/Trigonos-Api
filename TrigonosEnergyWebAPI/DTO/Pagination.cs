﻿namespace TrigonosEnergyWebAPI.DTO
{
    public class Pagination<T> where T : class
    {
        public int count { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public IReadOnlyList<T> Data { get; set; }
        public int PageCount { get; set; } 
        //public DateTime prueba { get; set; }
    }
    

    
}
