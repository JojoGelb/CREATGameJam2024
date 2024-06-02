using UnityEngine;
using UnityEngine.Rendering;

public class PaintManager : Singleton<PaintManager>{

    public Shader texturePaint;
    public Shader extendIslands;
    public Shader erasePaint;

    int prepareUVID = Shader.PropertyToID("_PrepareUV");
    int positionID = Shader.PropertyToID("_PainterPosition");
    int hardnessID = Shader.PropertyToID("_Hardness");
    int strengthID = Shader.PropertyToID("_Strength");
    int radiusID = Shader.PropertyToID("_Radius");
    int blendOpID = Shader.PropertyToID("_BlendOp");
    int colorID = Shader.PropertyToID("_PainterColor");
    int textureID = Shader.PropertyToID("_MainTex");
    int uvOffsetID = Shader.PropertyToID("_OffsetUV");
    int uvIslandsID = Shader.PropertyToID("_UVIslands");
    int eraseFeather = Shader.PropertyToID("_EraseFeather");

    Material paintMaterial;
    Material extendMaterial;
    private Material eraseMaterial;

    CommandBuffer command;

    protected override void Awake(){
        base.Awake();
        
        paintMaterial = new Material(texturePaint);
        extendMaterial = new Material(extendIslands);
        eraseMaterial = new Material(erasePaint);
        command = new CommandBuffer();
        command.name = "CommmandBuffer - " + gameObject.name;
    }

    public void initTextures(Paintable paintable){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        command.SetRenderTarget(mask);
        command.SetRenderTarget(extend);
        command.SetRenderTarget(support);

        paintMaterial.SetFloat(prepareUVID, 1);
        command.SetRenderTarget(uvIslands);
        command.DrawRenderer(rend, paintMaterial, 0);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }


    public void Paint(Paintable paintable, Vector3 pos, float radius = 1f, float hardness = .5f, float strength = .5f, Color? color = null){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        paintMaterial.SetFloat(prepareUVID, 0);
        paintMaterial.SetVector(positionID, pos);
        paintMaterial.SetFloat(hardnessID, hardness);
        paintMaterial.SetFloat(strengthID, strength);
        paintMaterial.SetFloat(radiusID, radius);
        paintMaterial.SetTexture(textureID, support); // _MainTex !!!!!!!!!!!!!!
        paintMaterial.SetColor(colorID, color ?? Color.red);
        extendMaterial.SetFloat(uvOffsetID, paintable.extendsIslandOffset);
        extendMaterial.SetTexture(uvIslandsID, uvIslands);

        // CALCUL MASQUE A PARTIR DU TAMPON (TexturePainter.shader)
        command.SetRenderTarget(mask);
        command.DrawRenderer(rend, paintMaterial, 0);
        // ENREGISTREMENT DU NOUVEAU TAMPON (copie du masque)
        command.SetRenderTarget(support);
        command.Blit(mask, support);

        // POST-TRAITEMENT SUR LE MASQUE (ExtendIslands.shader ->
        // servira à l'affichage mais pas au calcul du suivant car pas de tampon)
        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }

    public void Erase(Paintable paintable, Vector3 pos, float radius, float _eraseFeather = .2f){
        RenderTexture mask = paintable.getMask();
        RenderTexture uvIslands = paintable.getUVIslands();
        RenderTexture extend = paintable.getExtend();
        RenderTexture support = paintable.getSupport();
        Renderer rend = paintable.getRenderer();

        eraseMaterial.SetFloat(eraseFeather, _eraseFeather);
        eraseMaterial.SetVector(positionID, pos);
        eraseMaterial.SetFloat(radiusID, radius);
        eraseMaterial.SetTexture(textureID, support); // _MainTex !!!!!!!!!!!!!!
        eraseMaterial.SetColor(colorID, Color.red);

        // CALCUL MASQUE A PARTIR DU TAMPON (TexturePainter.shader)
        command.SetRenderTarget(mask);
        command.DrawRenderer(rend, eraseMaterial, 0);

        // ENREGISTREMENT DU NOUVEAU TAMPON (copie du masque)
        command.SetRenderTarget(support);
        command.Blit(mask, support);

        // POST-TRAITEMENT SUR LE MASQUE (ExtendIslands.shader ->
        // servira à l'affichage mais pas au calcul du suivant car pas de tampon)
        command.SetRenderTarget(extend);
        command.Blit(mask, extend, extendMaterial);

        Graphics.ExecuteCommandBuffer(command);
        command.Clear();
    }
}
