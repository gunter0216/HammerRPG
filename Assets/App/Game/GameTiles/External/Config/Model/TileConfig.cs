namespace App.Game.GameTiles.External.Config.Model
{
    public class TileConfig
    {
        private readonly string m_ID;
        private readonly string m_GenerationID;
        private readonly string m_SpriteKey;

        public string ID => m_ID;
        public string GenerationID => m_GenerationID;
        public string SpriteKey => m_SpriteKey;

        public TileConfig(string id, string generationID, string spriteKey)
        {
            m_ID = id;
            m_GenerationID = generationID;
            m_SpriteKey = spriteKey;
        }
    }
}