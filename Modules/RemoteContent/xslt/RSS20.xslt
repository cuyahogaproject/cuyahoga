<?xml version="1.0"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" version="1.0">
<xsl:output method="html" indent="yes" />

<xsl:template match="/">
  <xsl:apply-templates/>
</xsl:template>

<xsl:template match="channel">
  <ul>
    <xsl:apply-templates select="item"/>
  </ul>
</xsl:template>

<xsl:template match="item">
	<li>
        <a>
          <xsl:attribute name="href">
            <xsl:value-of select="link"/>
          </xsl:attribute>
          <xsl:value-of select="title" />
        </a>
	</li>
</xsl:template>

<xsl:template match="description">
  <div>
    <xsl:value-of select="."/>
  </div>
</xsl:template>

</xsl:stylesheet>


  