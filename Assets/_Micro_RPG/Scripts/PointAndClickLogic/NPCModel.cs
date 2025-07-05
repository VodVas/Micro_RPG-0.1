public class NPCModel
{
    private readonly string _npcId;
    private readonly float _interactionDistance;

    public string NpcId => _npcId;
    public float InteractionDistance => _interactionDistance;

    public NPCModel(string npcId, float interactionDistance)
    {
        _npcId = npcId;
        _interactionDistance = interactionDistance;
    }
}